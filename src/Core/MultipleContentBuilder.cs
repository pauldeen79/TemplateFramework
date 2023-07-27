namespace TemplateFramework.Core;

public sealed class MultipleContentBuilder : IMultipleContentBuilder
{
    private readonly List<IContentBuilder> _contentList;

    public MultipleContentBuilder()
    {
        _contentList = new List<IContentBuilder>();
    }

    public IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder)
    {
        Guard.IsNotNull(filename);

        var content = builder is null
            ? new ContentBuilder()
            : new ContentBuilder(builder);

        content.Filename = filename;
        content.SkipWhenFileExists = skipWhenFileExists;

        _contentList.Add(content);

        return content;
    }

    public IEnumerable<IContentBuilder> Contents => _contentList.AsReadOnly();

    public IMultipleContent Build() => new MultipleContent(Contents.Select(x => x.Build()));
}
