namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateFileManagerExtensionsTests
{
    protected Mock<ITemplateFileManager> CreateSut() => new();
    protected const string Filename = "Filename.txt";
    protected const string LastGeneratedFilesPath = "LastGeneratedFiles.txt";
    protected const bool Split = true;
}
