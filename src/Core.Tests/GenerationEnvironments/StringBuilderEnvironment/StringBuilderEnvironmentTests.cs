namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class StringBuilderEnvironmentTests
{
    internal Mock<IFileSystem> FileSystemMock { get; } = new();
    protected Mock<ICodeGenerationProvider> CodeGenerationProviderMock { get; } = new();
    protected StringBuilder Builder { get; } = new();
    
    protected StringBuilderEnvironment CreateSut() => new(FileSystemMock.Object, Builder);
}
