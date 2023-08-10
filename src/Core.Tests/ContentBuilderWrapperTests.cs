namespace TemplateFramework.Core.Tests;

public class ContentBuilderWrapperTests
{
    [Fact]
    public void Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new ContentBuilderWrapper(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Wraps_Instance_Correctly()
    {
        // Act
        var builder = new StringBuilder();
        var instance = new ContentBuilderWrapper(new MyContentBuilder { Filename = "Filename.txt", SkipWhenFileExists = true, Builder = builder });
        var compareTo = new MyContentBuilder { Filename = "Filename.txt", SkipWhenFileExists = true, Builder = builder };

        // Assert
        instance.Filename.Should().Be(compareTo.Filename);
        instance.SkipWhenFileExists.Should().Be(compareTo.SkipWhenFileExists.GetValueOrDefault());
        instance.Builder.Should().BeSameAs(builder);
    }

    [Fact]
    public void Wraps_Build_Result_Correctly()
    {
        // Act
        var builder = new StringBuilder();
        builder.Append("Some result");
        var instance = new ContentBuilderWrapper(new MyContentBuilder { Filename = "Filename.txt", SkipWhenFileExists = true, Builder = builder }).Build();
        var compareTo = new MyContentBuilder { Filename = "Filename.txt", SkipWhenFileExists = true, Builder = builder }.Build();

        // Assert
        instance.Filename.Should().Be(compareTo.Filename);
        instance.SkipWhenFileExists.Should().Be(compareTo.SkipWhenFileExists);
        instance.Contents.Should().Be(compareTo.Contents);
    }

    [Fact]
    public void Does_Not_Throw_On_Null_Filename()
    {
        // Arrange
        var instance = new ContentBuilderWrapper(new MyContentBuilder { Filename = null, SkipWhenFileExists = true, Builder = new StringBuilder() });

        // Act & Assert
        instance.Invoking(x => _ = x.Filename)
                .Should().NotThrow();
    }

    [Fact]
    public void Throws_On_Null_SkipWhenFileExists()
    {
        // Arrange
        var instance = new ContentBuilderWrapper(new MyContentBuilder { Filename = "Filename.txt", SkipWhenFileExists = null, Builder = new StringBuilder() });

        // Act & Assert
        instance.Invoking(x => _ = x.SkipWhenFileExists)
                .Should().Throw<InvalidOperationException>().WithMessage("SkipWhenFileExists of content builder TemplateFramework.Core.Tests.ContentBuilderWrapperTests+MyContentBuilder was null");
    }

    [Fact]
    public void Throws_On_Null_Builder()
    {
        // Arrange
        var instance = new ContentBuilderWrapper(new MyContentBuilder { Filename = "Filename.txt", SkipWhenFileExists = true, Builder = null });

        // Act & Assert
        instance.Invoking(x => _ = x.Builder)
                .Should().Throw<InvalidOperationException>().WithMessage("Builder of content builder TemplateFramework.Core.Tests.ContentBuilderWrapperTests+MyContentBuilder was null");
    }

    [Fact]
    public void Throws_When_WrappedInstance_Does_Not_Have_Filename_Property()
    {
        // Arrange
        var sut = new ContentBuilderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => x.Filename)
           .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Throws_When_WrappedInstance_Does_Not_Have_SkipWhenFilenameExists_Property()
    {
        // Arrange
        var sut = new ContentBuilderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => x.SkipWhenFileExists)
           .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Throws_When_WrappedInstance_Does_Not_Have_Builder_Property()
    {
        // Arrange
        var sut = new ContentBuilderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => x.Builder)
           .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Throws_When_WrappedInstance_Does_Not_Have_Build_Method()
    {
        // Arrange
        var sut = new ContentBuilderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => _ = x.Build())
           .Should().Throw<InvalidOperationException>();
    }

    private sealed class MyContentBuilder
    {
        public string? Filename { get; set; }
        public bool? SkipWhenFileExists { get; set; }
        public StringBuilder? Builder { get; set; }

        public IContent Build() => new MyContent();
    }

    private sealed class MyContent : IContent
    {
        public string Filename { get; set; } = "";
        public bool SkipWhenFileExists { get; set; }
        public string Contents { get; set; } = "";
    }
}
