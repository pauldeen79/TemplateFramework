namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    protected Mock<ITemplateEngine> EngineMock { get; } = new();
    protected Mock<ITemplateProvider> ProviderMock { get; } = new();
    protected const string DefaultFilename = "Filename.txt";

    protected TemplateContext CreateSut()
    {
        var rootTemplateContext = new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, template: this, model: 1);
        var parentTemplateContext = new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, template: this, parentContext: rootTemplateContext, model: "test model");
        return new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, template: this, parentContext: parentTemplateContext);
    }
}
