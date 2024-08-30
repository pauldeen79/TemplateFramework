namespace TemplateFramework.Core;

public class MultipleContentBuilder : MultipleContentBuilder<StringBuilder>
{
}

public class MultipleContentBuilder<T> : IMultipleContentBuilder<T> where T : class, new()
{
    private readonly List<IContentBuilder<T>> _contentList;

    public MultipleContentBuilder()
    {
        _contentList = new List<IContentBuilder<T>>();
    }

    public IContentBuilder<T> AddContent(string filename, bool skipWhenFileExists, T? builder)
    {
        Guard.IsNotNull(filename);

        var content = builder is null
            ? new ContentBuilder<T>()
            : new ContentBuilder<T>(builder);

        content.Filename = filename;
        content.SkipWhenFileExists = skipWhenFileExists;

        _contentList.Add(content);

        return content;
    }

    public IEnumerable<IContentBuilder<T>> Contents => _contentList.AsReadOnly();

    public IMultipleContent Build() => new MultipleContent(Contents.Select(x => x.Build()));
}
