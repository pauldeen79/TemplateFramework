namespace TemplateFramework.Abstractions;

public interface IMultipleContentBuilder
{
    string BasePath { get; set; }
    Encoding Encoding { get; set; }
    IEnumerable<IContentBuilder> Contents { get; }
    void SaveAll();
    void SaveLastGeneratedFiles(string lastGeneratedFilesPath);
    void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse);
    IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder);
    IMultipleContent Build();
}
