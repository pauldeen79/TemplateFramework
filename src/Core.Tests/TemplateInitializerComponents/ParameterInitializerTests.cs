namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ParameterInitializerTests
{
    protected ParameterInitializerComponent CreateSut() => new(ValueConverterMock);
    
    protected IValueConverter ValueConverterMock { get; } = Substitute.For<IValueConverter>();
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();

    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(ParameterInitializerComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize : ParameterInitializerTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Sets_AdditionalParameters_When_Template_Implements_IParameterizedTemplate()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            ValueConverterMock.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Fact]
        public void Converts_AdditionalParameter_When_Converter_Is_Available()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "?" };
            var template = new PlainTemplateWithAdditionalParameters();
            object? convertedValue = "Hello world!";
            ValueConverterMock.Convert(Arg.Any<object?>(), Arg.Any<Type>())
                              .Returns(convertedValue);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.AdditionalParameter.Should().BeEquivalentTo(convertedValue.ToString());
        }

        [Fact]
        public void Does_Not_Throw_On_Non_Existing_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!", NonExistingParameter = "Kaboom" };
            var template = new PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act & Assert
            sut.Invoking(x => x.Initialize(engineContext))
               .Should().NotThrow();
        }

        [Fact]
        public void Skips_Model_AdditionalParameter()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var additionalParameters = new { AdditionalParameter = "Hello world!", Model = "Ignored" };
            var template = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), model, new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            ValueConverterMock.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Model.Should().BeNull();
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Fact]
        public void Can_Inject_ViewModel_On_Template_Using_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.NonConstructableViewModel>(_ => { });
            var viewModel = new TestData.NonConstructableViewModel("Some value");
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters: new { ViewModel = viewModel });
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            ValueConverterMock.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.ViewModel.Should().BeSameAs(viewModel);
        }

        [Fact]
        public void Sets_AdditionalParameters_When_Template_Has_Public_Readable_And_Writable_Properties()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { Parameter = "Hello world!" };
            var template = new TestData.PocoParameterizedTemplate();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            ValueConverterMock.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);
            TemplateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string)) });

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Parameter.Should().Be(additionalParameters.Parameter);
        }

        [Fact]
        public void Skips_AdditionalParameters_When_Template_Does_Not_Implement_IParameterizedTemplate_And_Property_Is_Missing()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { WrongParameter = "Hello world!" };
            var template = new TestData.PocoParameterizedTemplate();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            ValueConverterMock.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);
            TemplateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string)) });

            // Act & Assert
            sut.Invoking(x => x.Initialize(engineContext)).Should().NotThrow();
        }
    }
}
