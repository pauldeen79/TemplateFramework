namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ParameterInitializerTests
{
    protected ParameterInitializer CreateSut() => new(ValueConverterMock.Object);
    
    protected Mock<IValueConverter> ValueConverterMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Converter()
        {
            // Act & Assert
            this.Invoking(_ => new ParameterInitializer(converter: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("converter");
        }
    }

    public class Initialize : ParameterInitializerTests
    {
        [Fact]
        public void Sets_AdditionalParameters_When_Template_Implements_IParameterizedTemplate()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(template, new StringBuilder(), DefaultFilename, additionalParameters);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Fact]
        public void Converts_AdditionalParameter_When_Converter_Is_Available()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "?" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            object? convertedValue = "Hello world!";
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>()))
                              .Returns(convertedValue);
            var request = new RenderTemplateRequest(template, new StringBuilder(), DefaultFilename, additionalParameters);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.AdditionalParameter.Should().BeEquivalentTo(convertedValue.ToString());
        }

        [Fact]
        public void Throws_On_Non_Existing_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!", NonExistingParameter = "Kaboom" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(template, new StringBuilder(), DefaultFilename, additionalParameters);

            // Act & Assert
            sut.Invoking(x => x.Initialize(request, TemplateEngineMock.Object))
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
            var request = new RenderTemplateRequest(template, model, new StringBuilder(), DefaultFilename, additionalParameters);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

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
            var request = new RenderTemplateRequest(template, new StringBuilder(), DefaultFilename, additionalParameters: new { ViewModel = viewModel });
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.ViewModel.Should().BeSameAs(viewModel);
        }
    }
}
