namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationProvider
{
    string Path { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }
    Encoding Encoding { get; }

    Type GetGeneratorType();
    Task<Result<object?>> CreateAdditionalParameters(CancellationToken cancellationToken);
    Task<Result<object?>> CreateModel(CancellationToken cancellationToken);
}
