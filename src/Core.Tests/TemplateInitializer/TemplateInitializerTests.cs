namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    protected TemplateInitializer CreateSut() => new(new[] { TemplateInitializerComponentMock.Object });
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    protected Mock<ITemplateInitializerComponent> TemplateInitializerComponentMock { get; } = new();
    protected Mock<IRenderTemplateRequest> RenderTemplateRequestMock { get; } = new();
    protected const string DefaultFilename = "DefaultFilename.txt";
}
