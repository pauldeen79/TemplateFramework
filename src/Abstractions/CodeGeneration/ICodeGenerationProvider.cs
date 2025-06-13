namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationProvider
{
    string Path { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }
    Encoding Encoding { get; }

    Type GetGeneratorType();
    Task<Result<object?>> CreateAdditionalParametersAsync(CancellationToken cancellationToken);
    Task<Result<object?>> CreateModelAsync(CancellationToken cancellationToken);
}
