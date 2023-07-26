namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationAssemblySettings : ICodeGenerationAssemblySettings
{
    public CodeGenerationAssemblySettings(string assemblyName)
        : this(assemblyName, false, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string assemblyName, string currentDirectory)
        : this(assemblyName, false, currentDirectory, null)
    {
    }

    public CodeGenerationAssemblySettings(string assemblyName, string currentDirectory, IEnumerable<string> classNameFilter)
        : this(assemblyName, false, currentDirectory, classNameFilter)
    {
    }

    public CodeGenerationAssemblySettings(string assemblyName, bool dryRun)
        : this(assemblyName, dryRun, null, null)
    {
    }

    public CodeGenerationAssemblySettings(string assemblyName, string currentDirectory, bool dryRun)
        : this(assemblyName, dryRun, currentDirectory, null)
    {
    }

    public CodeGenerationAssemblySettings(string assemblyName,
                                          bool dryRun,
                                          string? currentDirectory,
                                          IEnumerable<string>? classNameFilter)
    {
        Guard.IsNotNull(assemblyName);

        if (string.IsNullOrEmpty(currentDirectory))
        {
            currentDirectory = Directory.GetCurrentDirectory();
        }

        AssemblyName = assemblyName;
        CurrentDirectory = currentDirectory;
        ClassNameFilter = classNameFilter ?? Enumerable.Empty<string>();
        DryRun = dryRun;
    }

    public string AssemblyName { get; }

    public string CurrentDirectory { get; }

    public IEnumerable<string> ClassNameFilter { get; }

    public bool DryRun { get; }
}
