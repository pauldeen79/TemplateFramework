namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    protected StringBuilder StringBuilder { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();

    protected Mock<ITemplateInitializer> TemplateInitializerMock { get; } = new();
    protected Mock<ITemplateRenderer> TemplateRendererMock { get; } = new();

    protected TemplateEngine CreateSut() => new(TemplateInitializerMock.Object, new[] { TemplateRendererMock.Object });
}
