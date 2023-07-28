namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationSettings : ICodeGenerationSettings
{
    public CodeGenerationSettings(string basePath)
        : this(basePath, string.Empty, false)
    {
    }

    public CodeGenerationSettings(string basePath, string defaultFilename)
        : this(basePath, defaultFilename, false)
    {
    }

    public CodeGenerationSettings(string basePath, string defaultFilename, bool dryRun)
    {
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(defaultFilename);

        BasePath = basePath;
        DefaultFilename = defaultFilename;
        DryRun = dryRun;
    }

    public string BasePath { get; }
    public string DefaultFilename { get; }
    public bool DryRun { get; }
}
