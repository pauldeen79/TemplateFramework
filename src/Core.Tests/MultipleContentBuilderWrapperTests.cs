namespace TemplateFramework.Core.Tests;

public class MultipleContentBuilderWrapperTests
{
    [Fact]
    public void Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new MultipleContentBuilderWrapper(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Wraps_Instance_Correctly()
    {
        // Act
        var builder = new StringBuilder();
        var builder1 = new MyMultipleContentBuilder();
        builder1.AddContent("Filename.txt", true, builder);
        var instance = new MultipleContentBuilderWrapper(builder1);
        var compareTo = new MyMultipleContentBuilder();
        compareTo.AddContent("Filename.txt", true, builder);

        // Assert
        instance.Contents.Should().BeEquivalentTo(compareTo.Contents);
    }

    [Fact]
    public void Build_Wraps_Result_From_Wrapped_Instance_Build_Result()
    {
        // Arrange
        var builder = new StringBuilder();
        var builder1 = new MyMultipleContentBuilder();
        builder1.AddContent("Filename.txt", true, builder);
        var instance = new MultipleContentBuilderWrapper(builder1);
        var compareTo = new MyMultipleContentBuilder();
        compareTo.AddContent("Filename.txt", true, builder);
        var compareToResult = compareTo.Build();

        // Act
        var result = instance.Build();

        // Assert
        result.Should().BeEquivalentTo(compareToResult);
    }

    private class MyMultipleContentBuilder
    {
        private readonly List<IContentBuilder> _list = new();
        public IEnumerable<IContentBuilder>? Contents => _list;

        public IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder)
        {
            var contentBuilder = new MyContentBuilder
            {
                Filename = filename,
                SkipWhenFileExists = skipWhenFileExists,
                Builder = builder ?? new()
            };
            _list.Add(contentBuilder);
            return contentBuilder;
        }

        public IMultipleContent Build() => new MyMultipleContent
        {
            Contents = (Contents ?? Enumerable.Empty<IContentBuilder>())
            .Select(x => new MyContent
            {
                Contents = x.Builder.ToString(),
                Filename = x.Filename ?? string.Empty,
                SkipWhenFileExists = x.SkipWhenFileExists
            }).ToList().AsReadOnly()
        };
    }

    private sealed class MyContentBuilder : IContentBuilder
    {
        public string? Filename { get; set; }
        public bool SkipWhenFileExists { get; set; }

        public StringBuilder Builder { get; set; } = new();

        public IContent Build() => new MyContent
        {
            Contents = Builder.ToString(),
            Filename = Filename ?? string.Empty,
            SkipWhenFileExists = SkipWhenFileExists
        };
    }

    private sealed class MyMultipleContent : IMultipleContent
    {
        public IReadOnlyCollection<IContent> Contents { get; set; } = default!;
    }

    private sealed class MyContent : IContent
    {
        public string Filename { get; set; } = "";

        public bool SkipWhenFileExists { get; set; }

        public string Contents { get; set; } = "";
    }
}
