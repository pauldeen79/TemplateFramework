namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationSettings : ICodeGenerationSettings
{
    public CodeGenerationSettings(string basePath)
        : this(basePath, false, false, false)
    {
    }

    public CodeGenerationSettings(string basePath, bool dryRun)
        : this(basePath, false, false, dryRun)
    {
    }

    public CodeGenerationSettings(string basePath, bool generateMultipleFiles, bool dryRun)
        : this(basePath, generateMultipleFiles, false, dryRun)
    {
    }

    public CodeGenerationSettings(string basePath, bool generateMultipleFiles, bool skipWhenFileExists, bool dryRun)
    {
        Guard.IsNotNull(basePath);

        BasePath = basePath;
        GenerateMultipleFiles = generateMultipleFiles;
        SkipWhenFileExists = skipWhenFileExists;
        DryRun = dryRun;
    }

    public string BasePath { get; }
    public bool GenerateMultipleFiles { get; }
    public bool SkipWhenFileExists { get; }
    public bool DryRun { get; }
}
