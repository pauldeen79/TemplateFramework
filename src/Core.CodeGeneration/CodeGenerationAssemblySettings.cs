namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssemblySettings : ICodeGenerationAssemblySettings
{
    public CodeGenerationAssemblySettings(string basePath, string assemblyName)
        : this(basePath, assemblyName, false, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, string currentDirectory)
        : this(basePath, assemblyName, false, currentDirectory, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, string currentDirectory, IEnumerable<string> classNameFilter)
        : this(basePath, assemblyName, false, currentDirectory, classNameFilter)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, bool dryRun)
        : this(basePath, assemblyName, dryRun, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath, string assemblyName, string currentDirectory, bool dryRun)
        : this(basePath, assemblyName, dryRun, currentDirectory, null)
    {
    }

    public CodeGenerationAssemblySettings(string basePath,
                                          string assemblyName,
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

        BasePath = basePath;
        AssemblyName = assemblyName;
        CurrentDirectory = currentDirectory;
        ClassNameFilter = classNameFilter ?? Enumerable.Empty<string>();
        DryRun = dryRun;
    }

    public string BasePath { get; }
    public string AssemblyName { get; }

    public string CurrentDirectory { get; }

    public IEnumerable<string> ClassNameFilter { get; }

    public bool DryRun { get; }
}
