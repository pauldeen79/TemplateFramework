namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    public class Render : StringBuilderTemplateRendererTests
    {
        public Render()
        {
            StringBuilderTemplateRendererMock
                .Setup(x => x.TryRender(It.IsAny<object>(), It.IsAny<StringBuilder>()))
                .Returns(true);
        }

        [Fact]
        public void Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new Mock<IMultipleContentBuilder>().Object);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, TemplateProviderMock.Object, template);

            // Act & Assert
            sut.Invoking(x => x.Render(engineContext))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Renders_Template_Correctly_When_A_TemplateRenderer_Supports_The_Template()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, TemplateProviderMock.Object, template);

            // Act
            sut.Render(engineContext);

            // Assert
            StringBuilderTemplateRendererMock.Verify(x => x.TryRender(It.IsAny<object>(), It.IsAny<StringBuilder>()), Times.Once);
        }

        [Fact]
        public void Renders_Template_Using_ToString_When_No_TemplateRenderer_Supports_The_Template()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, TemplateProviderMock.Object, template);
            StringBuilderTemplateRendererMock
                .Setup(x => x.TryRender(It.IsAny<object>(), It.IsAny<StringBuilder>()))
                .Returns(false);

            // Act
            sut.Render(engineContext);

            // Assert
            generationEnvironment.ToString().Should().Be("TemplateFramework.Core.Tests.TestData+Template");
        }
    }
}
