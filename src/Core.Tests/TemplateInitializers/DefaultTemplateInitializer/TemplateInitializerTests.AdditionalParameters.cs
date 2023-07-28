namespace TemplateFramework.Core.Tests.TemplateInitializers;

public partial class TemplateInitializerTests
{
    public class AdditionalParameters : TemplateInitializerTests
    {
        [Fact]
        public void Sets_Model_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var template = new TestData.TemplateWithModel<string>(_ => { });
            var request = new RenderTemplateRequest(template, model, new StringBuilder(), DefaultFilename);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Model.Should().Be(model);
        }

        [Fact]
        public void Sets_AdditionalParameters_When_Template_Implements_IParameterizedTemplate()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(template, null, new StringBuilder(), DefaultFilename, additionalParameters);

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
            TemplateParameterConverterMock.Setup(x => x.TryConvert(It.IsAny<object?>(), It.IsAny<Type>(), out convertedValue))
                                          .Returns(true);
            var request = new RenderTemplateRequest(template, null, new StringBuilder(), DefaultFilename, additionalParameters);

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
            var request = new RenderTemplateRequest(template, null, new StringBuilder(), DefaultFilename, additionalParameters);

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

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Model.Should().Be(model);
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }
    }
}
