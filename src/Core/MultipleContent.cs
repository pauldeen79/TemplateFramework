namespace TemplateFramework.Core;

public class MultipleContent : IMultipleContent
{
    public MultipleContent(string basePath, Encoding encoding, IEnumerable<IContent> contents)
    {
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(contents);

        BasePath = basePath;
        Encoding = encoding;
        Contents = contents.ToList().AsReadOnly();
    }

    public string BasePath { get; }
    public Encoding Encoding { get; }
    public IReadOnlyCollection<IContent> Contents { get; }
}
