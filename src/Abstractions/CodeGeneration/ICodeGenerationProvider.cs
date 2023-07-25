namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationProvider
{
    bool SkipWhenFileExists { get; }
    string Path { get; }
    string DefaultFilename { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }

    void Initialize(bool skipWhenFileExists);
    object CreateGenerator();
    object? CreateAdditionalParameters();
    object? CreateModel();
}
