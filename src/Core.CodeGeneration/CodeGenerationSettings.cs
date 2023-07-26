namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationSettings : ICodeGenerationSettings
{
    public CodeGenerationSettings()
        : this(false)
    {
    }

    public CodeGenerationSettings(bool dryRun)
    {
        DryRun = dryRun;
    }

    public bool DryRun { get; }
}
