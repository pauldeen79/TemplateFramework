namespace TemplateFramework.Abstractions;

public interface IGenerationEnvironment
{
    GenerationEnvironmentType Type { get; }
    Task SaveContents(ICodeGenerationProvider provider, string basePath, string defaultFilename, CancellationToken cancellationToken);
}
