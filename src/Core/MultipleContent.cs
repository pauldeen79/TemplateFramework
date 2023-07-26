namespace TemplateFramework.Core;

internal sealed class MultipleContent : IMultipleContent
{
    public MultipleContent(string basePath, IEnumerable<IContent> contents)
    {
        BasePath = basePath;
        Contents = contents.ToList().AsReadOnly();
    }

    public string BasePath { get; }
    public IReadOnlyCollection<IContent> Contents { get; }
}
