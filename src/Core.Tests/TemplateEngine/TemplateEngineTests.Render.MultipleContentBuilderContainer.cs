namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilderContainer : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();
            IRenderTemplateRequest<object?> request = null!;

            // Act & Assert
            sut.Invoking(x => x.Render(request))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(request));
        }

        [Fact]
        public void Initializes_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateInitializerMock.Object, DefaultTemplateRenderers);
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilderContainer? generationEnvironment = MultipleContentBuilderContainerMock.Object;
            MultipleContentBuilderContainerMock.SetupGet(x => x.MultipleContentBuilder).Returns(MultipleContentBuilderMock.Object);
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest<object?>(template, generationEnvironment, additionalParameters);

            // Act
            sut.Render(request);

            // Assert
            TemplateInitializerMock.Verify(x => x.Initialize(request, sut), Times.Once);
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateInitializerMock.Object, new[] { TemplateRendererMock.Object });
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilderContainer? generationEnvironment = MultipleContentBuilderContainerMock.Object;
            MultipleContentBuilderContainerMock.SetupGet(x => x.MultipleContentBuilder).Returns(MultipleContentBuilderMock.Object);
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<IGenerationEnvironment>())).Returns(true);
            var request = new RenderTemplateRequest(template, generationEnvironment, additionalParameters);

            // Act
            sut.Render(request);

            // Assert
            TemplateRendererMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(req => req.Template == template && req.GenerationEnvironment.Type == GenerationEnvironmentType.MultipleContentBuilderContainer && req.DefaultFilename == string.Empty)), Times.Once);
        }
    }
}
