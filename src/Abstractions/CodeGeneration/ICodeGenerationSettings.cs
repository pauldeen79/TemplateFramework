namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationSettings
{
    string BasePath { get; }
    bool GenerateMultipleFiles { get; }
    bool SkipWhenFileExists { get; }
    bool DryRun { get; }
}
