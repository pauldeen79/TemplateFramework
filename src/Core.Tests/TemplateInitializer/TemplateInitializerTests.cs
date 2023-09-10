namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    protected TemplateInitializer CreateSut() => new(new[] { TemplateInitializerComponentMock });
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();
    protected ITemplateInitializerComponent TemplateInitializerComponentMock { get; } = Substitute.For<ITemplateInitializerComponent>();
    protected IRenderTemplateRequest RenderTemplateRequestMock { get; } = Substitute.For<IRenderTemplateRequest>();
    protected const string DefaultFilename = "DefaultFilename.txt";
}
