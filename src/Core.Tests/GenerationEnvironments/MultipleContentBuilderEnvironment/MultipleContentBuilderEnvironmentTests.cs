namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    protected IFileSystem FileSystemMock { get; } = Substitute.For<IFileSystem>();
    protected ICodeGenerationProvider CodeGenerationProviderMock { get; } = Substitute.For<ICodeGenerationProvider>();
    protected IMultipleContentBuilder MultipleContentBuilderMock { get; } = Substitute.For<IMultipleContentBuilder>();
    protected IMultipleContent MultipleContentMock { get; } = Substitute.For<IMultipleContent>();
    protected IContent ContentMock { get; } = Substitute.For<IContent>();
    protected IRetryMechanism RetryMechanism { get; } = new FastRetryMechanism();

    protected MultipleContentBuilderEnvironment CreateSut() => new(FileSystemMock, RetryMechanism, MultipleContentBuilderMock);

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
