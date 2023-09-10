namespace TemplateFramework.Core.Tests.Requests;

public partial class RenderTemplateRequestTests
{
    protected StringBuilder StringBuilder { get; } = new();
    protected IMultipleContentBuilder MultipleContentBuilderMock { get; } = Substitute.For<IMultipleContentBuilder>();
    protected IGenerationEnvironment GenerationEnvironmentMock { get; } = Substitute.For<IGenerationEnvironment>();
    protected ITemplateContext TemplateContextMock { get; } = Substitute.For<ITemplateContext>();

    protected const string DefaultFilename = "MyFile.txt";
    protected object AdditionalParameters { get; } = new object();
}
