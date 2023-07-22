namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationAssemblySettings : ICodeGenerationSettings
{
    string AssemblyName { get; }
    string CurrentDirectory { get; }
    IEnumerable<string> ClassNameFilter { get; }
}
