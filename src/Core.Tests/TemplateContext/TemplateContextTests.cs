namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    protected TemplateContext CreateSut()
    {
        var rootTemplateContext = new TemplateContext(template: this, model: 1);
        var parentTemplateContext = new TemplateContext(template: this, parentContext: rootTemplateContext, model: "test model");
        return new TemplateContext(template: this, parentContext: parentTemplateContext);
    }
}
