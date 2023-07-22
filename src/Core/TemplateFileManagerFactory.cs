namespace TemplateFramework.Core;

public class TemplateFileManagerFactory : ITemplateFileManagerFactory
{
    public ITemplateFileManager Create() => new TemplateFileManager();
}
