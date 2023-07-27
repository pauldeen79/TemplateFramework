namespace TemplateFramework.Abstractions;

public interface IGenerationEnvironment
{
    GenerationEnvironmentType Type { get; }
    void Process(ICodeGenerationProvider provider, string basePath, string defaultFilename);
}
