namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Supports : MultipleContentTemplateRendererTests
    {
        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var environmentMock = new Mock<IGenerationEnvironment>();
            environmentMock.SetupGet(x => x.Type).Returns(GenerationEnvironmentType.StringBuilder);

            // Act
            var result = sut.Supports(environmentMock.Object);

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

        [Fact]
        public void Returns_True_When_GenerationEnvironment_Is_MultipleContentBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(this), DefaultFilename, new Mock<IMultipleContentBuilder>().Object);

            // Act
            var result = sut.Supports(request.GenerationEnvironment);

            // Assert
            result.Should().BeTrue();
        }
    }
}
