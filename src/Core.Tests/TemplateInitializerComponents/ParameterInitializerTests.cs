namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ParameterInitializerTests
{
    protected ParameterInitializerComponent CreateSut() => new(ValueConverterMock.Object);
    
    protected Mock<IValueConverter> ValueConverterMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Converter()
        {
            // Act & Assert
            this.Invoking(_ => new ParameterInitializerComponent(converter: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("converter");
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
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

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
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>()))
                              .Returns(convertedValue);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.AdditionalParameter.Should().BeEquivalentTo(convertedValue.ToString());
        }

        [Fact]
        public void Throws_On_Non_Existing_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!", NonExistingParameter = "Kaboom" };
            var template = new PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);

            // Act & Assert
            sut.Invoking(x => x.Initialize(engineContext))
               .Should().Throw<NotSupportedException>().WithMessage("Unsupported template parameter: NonExistingParameter");
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
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

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
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

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
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);
            TemplateEngineMock.Setup(x => x.GetParameters(It.IsAny<object>())).Returns(new[] { new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string)) });

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
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);
            TemplateEngineMock.Setup(x => x.GetParameters(It.IsAny<object>())).Returns(new[] { new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string)) });

            // Act & Assert
            sut.Invoking(x => x.Initialize(engineContext)).Should().NotThrow();
        }
    }
}
