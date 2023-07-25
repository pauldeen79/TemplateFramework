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
            var contentMock = new Mock<IContent>();
            contentMock.SetupGet(x => x.Filename).Returns("Filename.txt");
            contentMock.SetupGet(x => x.Contents).Returns("Contents");
            contentMock.SetupGet(x => x.SkipWhenFileExists).Returns(true);
            var instance = new MultipleContent(TestData.BasePath, Encoding.Latin1, new[] { contentMock.Object });

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.Encoding.Should().Be(Encoding.Latin1);
            instance.Contents.Should().BeEquivalentTo(new[] { contentMock.Object });
        }
    }
}
