namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationSettings : ICodeGenerationSettings
{
    public CodeGenerationSettings(string basePath)
        : this(basePath, false)
    {
    }

    public CodeGenerationSettings(string basePath, bool dryRun)
    {
        Guard.IsNotNull(basePath);

        BasePath = basePath;
        DryRun = dryRun;
    }

    public string BasePath { get; }
    public bool DryRun { get; }
}
