namespace TemplateFramework.Core;

public class MultipleContentBuilder : IMultipleContentBuilder
{
    private readonly List<IContentBuilder> _contentList;

    public MultipleContentBuilder() : this(string.Empty)
    {
    }

    public MultipleContentBuilder(string basePath)
    {
        Guard.IsNotNull(basePath);
        
        _contentList = new List<IContentBuilder>();
        BasePath = basePath;
    }

    public string BasePath { get; set; }

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

    public IMultipleContent Build() => new MultipleContent(BasePath, Contents.Select(x => x.Build()));
}
