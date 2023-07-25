namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationSettings
{
    bool SkipWhenFileExists { get; }
    bool DryRun { get; }
}
