namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Requests;

public class CreateCompiledTemplateRequest : ICreateTemplateRequest
{
    public CreateCompiledTemplateRequest(string assemblyName, string className) : this(assemblyName, className, string.Empty)
    {
    }

    public CreateCompiledTemplateRequest(string assemblyName, string className, string currentDirectory)
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
