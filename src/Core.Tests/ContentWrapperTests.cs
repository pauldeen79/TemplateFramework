namespace TemplateFramework.Core.Tests;

public partial class ContentWrapperTests
{
    [Fact]
    public void Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new ContentWrapper(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Wraps_Instance_Correctly()
    {
        // Act
        var instance = new ContentWrapper(new MyContent { Filename = "Filename.txt", SkipWhenFileExists = true, Contents = "Contents" });
        var compareTo = new MyContent { Filename = "Filename.txt", SkipWhenFileExists = true, Contents = "Contents" };

        // Assert
        instance.Filename.Should().Be(compareTo.Filename);
        instance.SkipWhenFileExists.Should().Be(compareTo.SkipWhenFileExists.GetValueOrDefault());
        instance.Contents.Should().Be(compareTo.Contents);
    }

    [Fact]
    public void Throws_On_Null_Filename()
    {
        // Arrange
        var instance = new ContentWrapper(new MyContent());

        // Act & Assert
        instance.Invoking(x => _ = x.Filename)
                .Should().Throw<InvalidOperationException>().WithMessage("Filename of content TemplateFramework.Core.Tests.ContentWrapperTests+MyContent was null");
    }

    [Fact]
    public void Throws_On_Null_SkipWhenFileExists()
    {
        // Arrange
        var instance = new ContentWrapper(new MyContent());

        // Act & Assert
        instance.Invoking(x => _ = x.SkipWhenFileExists)
                .Should().Throw<InvalidOperationException>().WithMessage("SkipWhenFileExists of content TemplateFramework.Core.Tests.ContentWrapperTests+MyContent was null");
    }

    [Fact]
    public void Throws_On_Null_Contents()
    {
        // Arrange
        var instance = new ContentWrapper(new MyContent());

        // Act & Assert
        instance.Invoking(x => _ = x.Contents)
                .Should().Throw<InvalidOperationException>().WithMessage("Contents of content TemplateFramework.Core.Tests.ContentWrapperTests+MyContent was null");
    }

    [Fact]
    public void Throws_On_Filename_When_WrappedInstance_Does_Not_Have_This_Property()
    {
        // Arrange
        var instance = new ContentWrapper(new object());

        // Act & Assert
        instance.Invoking(x => _ = x.Filename)
                .Should().Throw<InvalidOperationException>().WithMessage("Filename of content System.Object was null");
    }

    [Fact]
    public void Throws_On_SkipWhenFileExists_When_WrappedInstance_Does_Not_Have_This_Property()
    {
        // Arrange
        var instance = new ContentWrapper(new object());

        // Act & Assert
        instance.Invoking(x => _ = x.SkipWhenFileExists)
                .Should().Throw<InvalidOperationException>().WithMessage("SkipWhenFileExists of content System.Object was null");
    }

    [Fact]
    public void Throws_On_Contents_When_WrappedInstance_Does_Not_Have_This_Property()
    {
        // Arrange
        var instance = new ContentWrapper(new object());

        // Act & Assert
        instance.Invoking(x => _ = x.Contents)
                .Should().Throw<InvalidOperationException>().WithMessage("Contents of content System.Object was null");
    }

    private sealed class MyContent
    {
        public string? Filename { get; set; }

        public bool? SkipWhenFileExists { get; set; }

        public string? Contents { get; set; }
    }
}
