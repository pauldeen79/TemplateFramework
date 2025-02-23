namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    public class Supports : StringBuilderTemplateRendererTests
    {
        [Fact]
        public void Returns_True_When_GenerationEnvironment_Is_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var environment = new StringBuilderEnvironment();

            // Act
            var result = sut.Supports(environment);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var environment = new MultipleStringContentBuilderEnvironment();

            // Act
            var result = sut.Supports(environment);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(null!);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
