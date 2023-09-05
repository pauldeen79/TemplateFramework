namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    protected Mock<IMultipleContentBuilderTemplateCreator> MultipleContentBuilderTemplateCreatorMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();

    protected MultipleContentTemplateRenderer CreateSut() => new(new[] { MultipleContentBuilderTemplateCreatorMock.Object });

    protected const string DefaultFilename = "MyFile.txt";
}
