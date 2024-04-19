namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationProvider
{
    string Path { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }
    Encoding Encoding { get; }

    Type GetGeneratorType();
    Task<object?> CreateAdditionalParameters();
    Task<object?> CreateModel();
}
