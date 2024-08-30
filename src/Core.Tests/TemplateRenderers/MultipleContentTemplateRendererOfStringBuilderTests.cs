namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateOfStringBuilderRendererTests
{
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();
    protected IMultipleContentBuilderTemplateCreator<StringBuilder> MultipleContentBuilderTemplateCreatorMock { get; } = Substitute.For<IMultipleContentBuilderTemplateCreator<StringBuilder>>();
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();

    protected MultipleContentOfStringBuilderTemplateRenderer CreateSut() => new(new[] { MultipleContentBuilderTemplateCreatorMock });

    protected const string DefaultFilename = "MyFile.txt";
}
