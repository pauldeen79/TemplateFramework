namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Render : MultipleContentTemplateRendererTests
    {
        public Render()
        {
            SingleContentTemplateRendererMock
                .Setup(x => x.Render(It.IsAny<ITemplateEngineContext>()))
                .Callback<ITemplateEngineContext>(req => ((StringBuilderEnvironment)req.GenerationEnvironment).Builder.Append(req.Template?.ToString()));
        }

        [Fact]
        public void Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(context: null!))
               .Should().Throw<ArgumentException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_When_GenerationEnvironemnt_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), DefaultFilename, new StringBuilder());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);

            // Act & Assert
            sut.Invoking(x => x.Render(engineContext))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Renders_MultipleContentBuilderTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = new Mock<IMultipleContentBuilderTemplate>();
            var generationEnvironment = new Mock<IMultipleContentBuilder>();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(templateMock.Object), DefaultFilename, generationEnvironment.Object);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, templateMock.Object);
            MultipleContentBuilderTemplateCreatorMock.Setup(x => x.TryCreate(It.IsAny<object>())).Returns(templateMock.Object);

            // Act
            sut.Render(engineContext);

            // Assert
            templateMock.Verify(x => x.Render(It.IsAny<IMultipleContentBuilder>()), Times.Once);
        }

        [Fact]
        public void Renders_To_MultipleContentBuilder_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = new Mock<IMultipleContentBuilder>();
            var contentBuilderMock = new Mock<IContentBuilder>();
            generationEnvironment.Setup(x => x.AddContent(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<StringBuilder?>()))
                                 .Returns<string, bool, StringBuilder?>((filename, skipWhenFileExists, b) =>
                                 {
                                     contentBuilderMock.SetupGet(x => x.Builder).Returns(b ?? new StringBuilder());

                                     return contentBuilderMock.Object;
                                 });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), DefaultFilename, generationEnvironment.Object);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, template);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<TemplateInstanceIdentifier>())).Returns(template);

            // Act
            sut.Render(engineContext);

            // Assert
            contentBuilderMock.Object.Builder.Should().NotBeNull();
            contentBuilderMock.Object.Builder.ToString().Should().Be("TemplateFramework.Core.Tests.TestData+TextTransformTemplate");
        }
    }
}
