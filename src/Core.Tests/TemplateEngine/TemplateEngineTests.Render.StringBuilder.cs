namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_StringBuilder : TemplateEngineTests
    {
        public Render_StringBuilder()
        {
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<IGenerationEnvironment>())).Returns(true);
        }

        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(request: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Initializes_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            StringBuilder? builder = StringBuilder;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest(template, additionalParameters, builder);

            // Act
            sut.Render(request);

            // Assert
            TemplateInitializerMock.Verify(x => x.Initialize(request, sut), Times.Once);
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            StringBuilder? generationEnvironment = StringBuilder;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<IGenerationEnvironment>())).Returns(true);
            var request = new RenderTemplateRequest(template, additionalParameters, generationEnvironment);

            // Act
            sut.Render(request);

            // Assert
            TemplateRendererMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(req => req.Template == template && req.GenerationEnvironment.Type == GenerationEnvironmentType.StringBuilder && req.DefaultFilename == string.Empty)), Times.Once);
        }
    }
}
