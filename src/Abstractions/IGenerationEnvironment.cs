namespace TemplateFramework.Abstractions;

public interface IGenerationEnvironment
{
    Task SaveContents(ICodeGenerationProvider provider, string basePath, string defaultFilename, CancellationToken cancellationToken);
}
