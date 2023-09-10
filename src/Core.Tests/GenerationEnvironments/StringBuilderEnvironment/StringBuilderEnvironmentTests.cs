namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class StringBuilderEnvironmentTests
{
    protected IFileSystem FileSystemMock { get; } = Substitute.For<IFileSystem>();
    protected ICodeGenerationProvider CodeGenerationProviderMock { get; } = Substitute.For<ICodeGenerationProvider>();
    protected StringBuilder Builder { get; } = new();
    
    protected StringBuilderEnvironment CreateSut() => new(FileSystemMock, Builder);
}
