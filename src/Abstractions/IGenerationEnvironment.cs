namespace TemplateFramework.Abstractions;

public interface IGenerationEnvironment
{
    GenerationEnvironmentType Type { get; }
    void SaveContents(ICodeGenerationProvider provider, string basePath, string defaultFilename);
}
