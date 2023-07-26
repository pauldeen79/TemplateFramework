namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    protected Mock<IFileSystem> FileSystemMock { get; } = new();
    protected Mock<ICodeGenerationProvider> CodeGenerationProviderMock { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IMultipleContent> MultipleContentMock { get; } = new();
    protected Mock<IContent> ContentMock { get; } = new();

    protected MultipleContentBuilderEnvironment CreateSut() => new(FileSystemMock.Object, MultipleContentBuilderMock.Object);
}
