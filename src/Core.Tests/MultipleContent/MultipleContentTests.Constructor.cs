namespace TemplateFramework.Core.Tests;

public class MultipleContentTests
{
    public class Constructor
    {
        [Fact]
        public void Throw_On_Null_BasePath()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContent(basePath: null!, Encoding.UTF8, Enumerable.Empty<IContent>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }

        [Fact]
        public void Throw_On_Null_Contents()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContent(basePath: TestData.BasePath, Encoding.UTF8, contents: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("contents");
        }

        [Fact]
        public void Creates_Instance_Correctly()
        {
            // Act
            var instance = new MultipleContent(TestData.BasePath, Encoding.Latin1, new[] { new Content("Contents", true, "Filename.txt") });

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.Encoding.Should().Be(Encoding.Latin1);
            instance.Contents.Should().BeEquivalentTo(new[] { new Content("Contents", true, "Filename.txt") });
        }
    }
}
