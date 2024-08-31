namespace TemplateFramework.Core;

public class MultipleContentBuilder : MultipleContentBuilder<StringBuilder>
{
    public override IContentBuilder<StringBuilder> AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder)
    {
        Guard.IsNotNull(filename);

        var content = builder is null
            ? new ContentBuilderOfStringBuilder()
            : new ContentBuilderOfStringBuilder(builder);

        content.Filename = filename;
        content.SkipWhenFileExists = skipWhenFileExists;

        ContentList.Add(content);

        return content;
    }
}

public abstract class MultipleContentBuilder<T> : IMultipleContentBuilder<T> where T : class, new()
{
    protected Collection<IContentBuilder<T>> ContentList { get; }

    protected MultipleContentBuilder()
    {
        ContentList = new Collection<IContentBuilder<T>>();
    }

    public abstract IContentBuilder<T> AddContent(string filename, bool skipWhenFileExists, T? builder);

    public IEnumerable<IContentBuilder<T>> Contents => ContentList.AsReadOnly();

    public IMultipleContent Build() => new MultipleContent(Contents.Select(x => x.Build()));
}
