namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Requests;

public class CreateCompiledTemplateRequest : ICreateTemplateRequest
{
    public CreateCompiledTemplateRequest(string assemblyName, string className)
    {
        Guard.IsNotNullOrEmpty(assemblyName);
        Guard.IsNotNullOrEmpty(className);

        AssemblyName = assemblyName;
        ClassName = className;
    }

    public string AssemblyName { get; }
    public string ClassName { get; }
}
