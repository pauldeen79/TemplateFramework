namespace TemplateFramework.Abstractions;

public interface IMultipleContentBuilder
{
    string BasePath { get; set; }
    IEnumerable<IContentBuilder> Contents { get; }
    IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder);
    IMultipleContent Build();
}
