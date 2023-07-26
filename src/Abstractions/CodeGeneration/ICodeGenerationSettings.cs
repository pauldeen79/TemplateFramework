namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationSettings
{
    string BasePath { get; }
    bool DryRun { get; }
}
