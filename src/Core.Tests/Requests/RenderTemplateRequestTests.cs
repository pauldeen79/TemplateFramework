namespace TemplateFramework.Core.Tests.Requests;

public partial class RenderTemplateRequestTests
{
    protected StringBuilder StringBuilder { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IGenerationEnvironment> GenerationEnvironmentMock { get; } = new();

    protected const string DefaultFilename = "MyFile.txt";
}
