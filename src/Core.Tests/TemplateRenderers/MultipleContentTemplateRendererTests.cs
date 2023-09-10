namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();
    protected IMultipleContentBuilderTemplateCreator MultipleContentBuilderTemplateCreatorMock { get; } = Substitute.For<IMultipleContentBuilderTemplateCreator>();
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();

    protected MultipleContentTemplateRenderer CreateSut() => new(new[] { MultipleContentBuilderTemplateCreatorMock });

    protected const string DefaultFilename = "MyFile.txt";
}
