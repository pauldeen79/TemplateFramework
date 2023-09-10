namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    protected ITemplateEngine EngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateProvider ProviderMock { get; } = Substitute.For<ITemplateProvider>();
    protected const string DefaultFilename = "Filename.txt";

    protected TemplateContext CreateSut()
    {
        var rootTemplateContext = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, identifier: new TemplateInstanceIdentifier(this), template: this, model: 1);
        var parentTemplateContext = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, identifier: new TemplateInstanceIdentifier(this), template: this, parentContext: rootTemplateContext, model: "test model");
        return new TemplateContext(EngineMock, ProviderMock, DefaultFilename, identifier: new TemplateInstanceIdentifier(this), template: this, parentContext: parentTemplateContext);
    }
}
