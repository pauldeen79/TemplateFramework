namespace TemplateFramework.Abstractions;

public interface IGenerationEnvironment
{
    Task<Result> SaveContentsAsync(ICodeGenerationProvider provider, string basePath, string defaultFilename, CancellationToken cancellationToken);
}
