namespace TemplateFramework.Core.Tests;

public class MultipleContentWrapperTests
{
    [Fact]
    public void Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new MultipleContentWrapper(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Wraps_Instance_Correctly()
    {
        // Act
        var contents = new[] { new MyContent { Contents = "Contents", Filename = "Filename.txt", SkipWhenFileExists = true } }.ToList().AsReadOnly();
        var instance = new MultipleContentWrapper(new MyMultipleContent { Contents = contents });
        var compareTo = new MyMultipleContent { Contents = contents };

        // Assert
        instance.Contents.Should().BeEquivalentTo(compareTo.Contents);
    }

    [Fact]
    public void Throws_When_Instance_Does_Not_Have_Contents_Property()
    {
        // Act & Assert
        this.Invoking(_ => new MultipleContentWrapper(new object()))
            .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Throws_When_Instance_Contents_Is_Null()
    {
        // Act & Assert
        this.Invoking(_ => new MultipleContentWrapper(new MyMultipleContent { Contents = null }))
            .Should().Throw<InvalidOperationException>();
    }

    private sealed class MyMultipleContent
    {
        public IReadOnlyCollection<IContent>? Contents { get; set; }
    }

    private sealed class MyContent : IContent
    {
        public string Filename { get; set; } = "";

        public bool SkipWhenFileExists { get; set; }

        public string Contents { get; set; } = "";
    }
}
