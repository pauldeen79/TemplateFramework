namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationSettings
{
    string BasePath { get; }
    string DefaultFilename { get; }
    bool DryRun { get; }
}
