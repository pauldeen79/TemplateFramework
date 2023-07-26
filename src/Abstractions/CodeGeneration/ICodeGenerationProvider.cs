namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationProvider
{
    bool SkipWhenFileExists { get; }
    string Path { get; }
    string DefaultFilename { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }

    object CreateGenerator();
    object? CreateAdditionalParameters();
    object? CreateModel();
}
