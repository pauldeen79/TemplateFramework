namespace TemplateFramework.Abstractions;

public interface IGenerationEnvironment
{
    GenerationEnvironmentType Type { get; }
    void Process(ICodeGenerationProvider provider, bool dryRun);
}
