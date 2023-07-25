namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    public class Render : StringBuilderTemplateRendererTests
    {
        [Fact]
        public void Throws_When_Request_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(request: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var request = new RenderTemplateRequest(new TestData.Template(_ => { }), new Mock<IMultipleContentBuilder>().Object, DefaultFilename);

            // Act & Assert
            sut.Invoking(x => x.Render(request))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(template, generationEnvironment, DefaultFilename);

            // Act
            sut.Render(request);

            // Assert
            generationEnvironment.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_TextTransformTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(template, generationEnvironment, DefaultFilename);

            // Act
            sut.Render(request);

            // Assert
            generationEnvironment.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_NonTemplateType_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplate(() => "Hello world!");
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(template, generationEnvironment, DefaultFilename);

            // Act
            sut.Render(request);

            // Assert
            generationEnvironment.ToString().Should().Be("Hello world!");
        }
    }
}
