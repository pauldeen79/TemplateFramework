namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    protected IBuilderTemplateRenderer<StringBuilder> StringBuilderTemplateRendererMock { get; } = Substitute.For<IBuilderTemplateRenderer<StringBuilder>>();
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();

    protected StringBuilderTemplateRenderer CreateSut() => new([StringBuilderTemplateRendererMock]);
}
