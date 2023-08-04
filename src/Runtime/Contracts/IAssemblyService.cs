namespace TemplateFramework.Runtime.Contracts;

public interface IAssemblyService
{
    Assembly GetAssembly(string assemblyName, string currentDirectory);
}
