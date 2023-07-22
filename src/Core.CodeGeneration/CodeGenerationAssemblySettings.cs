namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationAssemblySettings : ICodeGenerationAssemblySettings
{
    public CodeGenerationAssemblySettings(string basePath, string assemblyName)
        : this(basePath, assemblyName, false, false, false, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, string currentDirectory)
        : this(basePath, assemblyName, false, false, false, currentDirectory, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, string currentDirectory, IEnumerable<string> classNameFilter)
        : this(basePath, assemblyName, false, false, false, currentDirectory, classNameFilter)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, bool dryRun)
        : this(basePath, assemblyName, false, false, dryRun, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, bool generateMultipleFiles, bool dryRun)
        : this(basePath, assemblyName, generateMultipleFiles, false, dryRun, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath,
                                          string assemblyName,
                                          bool generateMultipleFiles,
                                          bool skipWhenFileExists,
                                          bool dryRun,
                                          string? currentDirectory,
                                          IEnumerable<string>? classNameFilter)
    {
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(assemblyName);

        if (string.IsNullOrEmpty(currentDirectory))
        {
            currentDirectory = Directory.GetCurrentDirectory();
        }

        AssemblyName = assemblyName;
        CurrentDirectory = currentDirectory;
        ClassNameFilter = classNameFilter ?? Enumerable.Empty<string>();
        BasePath = basePath;
        GenerateMultipleFiles = generateMultipleFiles;
        SkipWhenFileExists = skipWhenFileExists;
        DryRun = dryRun;
    }

    public string AssemblyName { get; }

    public string CurrentDirectory { get; }

    public IEnumerable<string> ClassNameFilter { get; }

    public string BasePath { get; }

    public bool GenerateMultipleFiles { get; }

    public bool SkipWhenFileExists { get; }

    public bool DryRun { get; }
}
