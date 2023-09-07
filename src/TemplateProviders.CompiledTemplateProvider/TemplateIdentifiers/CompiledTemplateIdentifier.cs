namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.TemplateIdentifiers;

public sealed class CompiledTemplateIdentifier : ITemplateIdentifier
{
    public CompiledTemplateIdentifier(string assemblyName, string className) : this(assemblyName, className, null)
    {
    }

    public CompiledTemplateIdentifier(string assemblyName, string className, string? currentDirectory)
    {
        Guard.IsNotNullOrEmpty(assemblyName);
        Guard.IsNotNullOrEmpty(className);

        if (string.IsNullOrEmpty(currentDirectory))
        {
            currentDirectory = Directory.GetCurrentDirectory();
        }

        AssemblyName = assemblyName;
        ClassName = className;
        CurrentDirectory = currentDirectory;
    }

    public string AssemblyName { get; }
    public string ClassName { get; }
    public string CurrentDirectory { get; }
}
