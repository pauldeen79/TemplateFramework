namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    protected Mock<IStringBuilderTemplateRenderer> StringBuilderTemplateRendererMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();

    protected StringBuilderTemplateRenderer CreateSut() => new(new[] { StringBuilderTemplateRendererMock.Object });
}
