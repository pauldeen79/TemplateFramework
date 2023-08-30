namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilder : TemplateEngineTests
    {
        public Render_MultipleContentBuilder()
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
            var template = new PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment, additionalParameters);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<TemplateInstanceIdentifier>())).Returns(template);

            // Act
            sut.Render(request);

            // Assert
            TemplateInitializerMock.Verify(x => x.Initialize(It.IsAny<ITemplateEngineContext>()), Times.Once);
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<IGenerationEnvironment>())).Returns(true);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, generationEnvironment);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<TemplateInstanceIdentifier>())).Returns(template);

            // Act
            sut.Render(request);

            // Assert
            TemplateRendererMock.Verify(x => x.Render(It.Is<ITemplateEngineContext>(req =>
                req.Identifier is TemplateInstanceIdentifier
                && req.GenerationEnvironment.Type == GenerationEnvironmentType.MultipleContentBuilder
                && req.DefaultFilename == string.Empty)), Times.Once);
        }
    }
}
