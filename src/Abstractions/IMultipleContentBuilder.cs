namespace TemplateFramework.Abstractions;

public interface IMultipleContentBuilder
{
    string BasePath { get; set; }
    IEnumerable<IContentBuilder> Contents { get; }
    void SaveAll(Encoding encoding);
    void SaveLastGeneratedFiles(string lastGeneratedFilesPath, Encoding encoding);
    void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse, Encoding encoding);
    IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder);
    IMultipleContent Build();
}
