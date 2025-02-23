namespace TemplateFramework.Core.Tests;

public class IndentedStringBuilderTests
{
    [Fact]
    public void Just_Works()
    {
        // Arrange
        var sut = new IndentedStringBuilder();

        // Act
        sut.AppendLine("Hello world");
        sut.IncrementIndent();
        sut.AppendLine("Two");
        sut.DecrementIndent();
        sut.AppendLine("Back to normal");
        var actual = sut.ToString();

        // Assert
        actual.ShouldBe(@"Hello world;
    Two
Back to normal
");
    }
}
