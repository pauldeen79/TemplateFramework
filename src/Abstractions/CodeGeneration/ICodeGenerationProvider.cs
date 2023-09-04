namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationProvider
{
    string Path { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }
    Encoding Encoding { get; }

    Type GetGeneratorType();
    object? CreateAdditionalParameters();
    object? CreateModel();
}
