namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationSettings : ICodeGenerationSettings
{
    public CodeGenerationSettings()
        : this(false, false)
    {
    }

    public CodeGenerationSettings(bool dryRun)
        : this(false, dryRun)
    {
    }

    public CodeGenerationSettings(bool skipWhenFileExists, bool dryRun)
    {
        SkipWhenFileExists = skipWhenFileExists;
        DryRun = dryRun;
    }

    public bool SkipWhenFileExists { get; }
    public bool DryRun { get; }
}
