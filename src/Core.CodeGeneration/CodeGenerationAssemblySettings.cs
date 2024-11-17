namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssemblySettings : ICodeGenerationAssemblySettings
{
    public CodeGenerationAssemblySettings(string basePath, string defaultFilename, string assemblyName)
        : this(basePath, defaultFilename, assemblyName, false, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string defaultFilename, string assemblyName, string currentDirectory)
        : this(basePath, defaultFilename, assemblyName, false, currentDirectory, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string defaultFilename, string assemblyName, string currentDirectory, IEnumerable<string> classNameFilter)
        : this(basePath, defaultFilename, assemblyName, false, currentDirectory, classNameFilter)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string defaultFilename, string assemblyName, bool dryRun)
        : this(basePath, defaultFilename, assemblyName, dryRun, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string defaultFilename, string assemblyName, string currentDirectory, bool dryRun)
        : this(basePath, defaultFilename, assemblyName, dryRun, currentDirectory, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath,
                                          string defaultFilename,
                                          string assemblyName,
                                          bool dryRun,
                                          string? currentDirectory,
                                          IEnumerable<string>? classNameFilter)
    {
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(defaultFilename);
        Guard.IsNotNull(assemblyName);

        if (string.IsNullOrEmpty(currentDirectory))
        {
            currentDirectory = Directory.GetCurrentDirectory();
        }

        BasePath = basePath;
        DefaultFilename = defaultFilename;
        AssemblyName = assemblyName;
        CurrentDirectory = currentDirectory;
        ClassNameFilter = classNameFilter ?? Enumerable.Empty<string>();
        DryRun = dryRun;
    }

    public string BasePath { get; }
    public string DefaultFilename { get; }
    public bool DryRun { get; }

    public string AssemblyName { get; }
    public string CurrentDirectory { get; }
    public IEnumerable<string> ClassNameFilter { get; }
}
