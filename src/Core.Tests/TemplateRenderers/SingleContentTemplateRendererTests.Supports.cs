namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class SingleContentTemplateRendererTests
{
    public class Supports : SingleContentTemplateRendererTests
    {
        [Fact]
        public void Returns_True_When_GenerationEnvironment_Is_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var request = new RenderTemplateRequest(this, new StringBuilder(), DefaultFilename);

            // Act
            var result = sut.Supports(request.GenerationEnvironment);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var request = new RenderTemplateRequest(this, new Mock<IMultipleContentBuilder>().Object, DefaultFilename);

            // Act
            var result = sut.Supports(request.GenerationEnvironment);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }
    }
}
