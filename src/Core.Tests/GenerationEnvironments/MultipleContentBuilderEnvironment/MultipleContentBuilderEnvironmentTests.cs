namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    protected Mock<IFileSystem> FileSystemMock { get; } = new();
    protected Mock<ICodeGenerationProvider> CodeGenerationProviderMock { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IMultipleContent> MultipleContentMock { get; } = new();
    protected Mock<IContent> ContentMock { get; } = new();
    protected IRetryMechanism RetryMechanism { get; } = new FastRetryMechanism();

    protected MultipleContentBuilderEnvironment CreateSut() => new(FileSystemMock.Object, RetryMechanism, MultipleContentBuilderMock.Object);

    protected IEnumerable<IContent> CreateContents(bool skipWhenFileExists = false)
    {
        var builder = new MultipleContentBuilder();
        var c1 = builder.AddContent("File1.txt", skipWhenFileExists: skipWhenFileExists);
        c1.Builder.AppendLine("Test1");
        var c2 = builder.AddContent("File2.txt", skipWhenFileExists: skipWhenFileExists);
        c2.Builder.AppendLine("Test2");
        return builder.Build().Contents;
    }

    private sealed class FastRetryMechanism : RetryMechanism
    {
        protected override int WaitTimeInMs => 1;
    }
}
