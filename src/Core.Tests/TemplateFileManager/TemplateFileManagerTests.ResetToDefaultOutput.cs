namespace TemplateFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class ResetToDefaultOutput : TemplateFileManagerTests
    {
        [Fact]
        public void Resets_Builder_To_OriginalContent()
        {
            // Arrange
            StringBuilder.Append("OriginalContent");
            var sut = CreateSutWithRealMultipleContentBuilder();
            sut.GenerationEnvironment.Append("Ignored");
            sut.StartNewFile("MyFile.txt").Append("Also ignored");

            // Act
            sut.ResetToDefaultOutput();

            // Assert
            sut.GenerationEnvironment.ToString().Should().Be("OriginalContent");
        }
    }
}
