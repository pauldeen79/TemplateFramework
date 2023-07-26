namespace TemplateFramework.Abstractions;

public interface IMultipleContentBuilder
{
    IEnumerable<IContentBuilder> Contents { get; }
    IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder);
    IMultipleContent Build();
}
