namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    protected IStringBuilderTemplateRenderer StringBuilderTemplateRendererMock { get; } = Substitute.For<IStringBuilderTemplateRenderer>();
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();

    protected StringBuilderTemplateRenderer CreateSut() => new(new[] { StringBuilderTemplateRendererMock });
}
