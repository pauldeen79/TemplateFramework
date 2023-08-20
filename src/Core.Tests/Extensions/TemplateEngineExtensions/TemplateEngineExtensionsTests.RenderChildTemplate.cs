namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplate : TemplateEngineExtensionsTests
    {
        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_1()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }
    }
}
