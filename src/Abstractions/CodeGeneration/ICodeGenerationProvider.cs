namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationProvider
{
    string Path { get; }
    string DefaultFilename { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }
    Encoding Encoding { get; }

    object CreateGenerator();
    object? CreateAdditionalParameters();
    object? CreateModel();
}
