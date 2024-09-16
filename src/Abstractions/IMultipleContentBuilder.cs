namespace TemplateFramework.Abstractions;

public interface IMultipleContentBuilder<T> where T : class
{
    IEnumerable<IContentBuilder<T>> Contents { get; }
    IContentBuilder<T> AddContent(string filename, bool skipWhenFileExists, T? builder);
    IMultipleContent Build();
}
