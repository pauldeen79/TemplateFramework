namespace TemplateFramework.Runtime.Abstractions;

public interface IAssemblyService
{
    Assembly GetAssembly(string assemblyName, string currentDirectory);
}
