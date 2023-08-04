namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Render : MultipleContentTemplateRendererTests
    {
        [Fact]
        public void Throws_When_Request_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(request: null!))
               .Should().Throw<ArgumentException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_When_GenerationEnvironemnt_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var request = new RenderTemplateRequest(new TestData.Template(_ => { }), DefaultFilename, new StringBuilder());

            // Act & Assert
            sut.Invoking(x => x.Render(request))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Renders_MultipleContentBuilderTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = new Mock<IMultipleContentBuilderTemplate>();
            var generationEnvironment = new Mock<IMultipleContentBuilder>();
            var request = new RenderTemplateRequest(templateMock.Object, DefaultFilename, generationEnvironment.Object);
            MultipleContentBuilderTemplateCreatorMock.Setup(x => x.TryCreate(It.IsAny<object>())).Returns(templateMock.Object);

            // Act
            sut.Render(request);

            // Assert
            templateMock.Verify(x => x.Render(It.IsAny<IMultipleContentBuilder>()), Times.Once);
        }

        [Fact]
        public void Renders_To_MultipleContentBuilder_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = new Mock<IMultipleContentBuilder>();
            var contentBuilderMock = new Mock<IContentBuilder>();
            generationEnvironment.Setup(x => x.AddContent(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<StringBuilder?>()))
                                 .Returns<string, bool, StringBuilder?>((filename, skipWhenFileExists, b) =>
                                 {
                                     contentBuilderMock.SetupGet(x => x.Builder).Returns(b ?? new StringBuilder());

                                     return contentBuilderMock.Object;
                                 });
            var request = new RenderTemplateRequest(templateMock, DefaultFilename, generationEnvironment.Object);

            // Act
            sut.Render(request);

            // Assert
            contentBuilderMock.Object.Builder.Should().NotBeNull();
            contentBuilderMock.Object.Builder.ToString().Should().Be("Hello world!");
        }
    }
}
