namespace TemplateFramework.Core;

internal sealed class MultipleContent : IMultipleContent
{
    public MultipleContent(string basePath, Encoding encoding, IEnumerable<IContent> contents)
    {
        BasePath = basePath;
        Encoding = encoding;
        Contents = contents.ToList().AsReadOnly();
    }

    public string BasePath { get; }
    public Encoding Encoding { get; }
    public IReadOnlyCollection<IContent> Contents { get; }
}
