namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    protected StringBuilder StringBuilder { get; } = new();
    protected IMultipleContentBuilder<StringBuilder> MultipleContentBuilderMock { get; } = Substitute.For<IMultipleContentBuilder<StringBuilder>>();

    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();
    protected ITemplateInitializer TemplateInitializerMock { get; } = Substitute.For<ITemplateInitializer>();
    protected ITemplateParameterExtractor TemplateParameterExtractorMock { get; } = Substitute.For<ITemplateParameterExtractor>();
    protected ITemplateRenderer TemplateRendererMock { get; } = Substitute.For<ITemplateRenderer>();

    protected TemplateEngine CreateSut() => new(TemplateProviderMock, TemplateInitializerMock, TemplateParameterExtractorMock, [TemplateRendererMock]);
}
