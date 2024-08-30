namespace TemplateFramework.Abstractions;

//TODO: Review if we want this. This is purely a backwards compatibility thing.
public interface IMultipleContentBuilder : IMultipleContentBuilder<StringBuilder>
{
}

public interface IMultipleContentBuilder<T> where T : class
{
    IEnumerable<IContentBuilder<T>> Contents { get; }
    IContentBuilder<T> AddContent(string filename, bool skipWhenFileExists, T? builder);
    IMultipleContent Build();
}
