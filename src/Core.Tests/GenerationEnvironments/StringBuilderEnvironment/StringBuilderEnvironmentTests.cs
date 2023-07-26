namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class StringBuilderEnvironmentTests
{
    protected Mock<IFileSystem> FileSystemMock { get; } = new();
    protected StringBuilder Builder { get; } = new();
    
    protected StringBuilderEnvironment CreateSut() => new(FileSystemMock.Object, Builder);
}
