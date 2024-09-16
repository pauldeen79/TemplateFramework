namespace TemplateFramework.Abstractions;

public interface IGenerationEnvironment
{
    Task<Result> SaveContents(ICodeGenerationProvider provider, string basePath, string defaultFilename, CancellationToken cancellationToken);
}
