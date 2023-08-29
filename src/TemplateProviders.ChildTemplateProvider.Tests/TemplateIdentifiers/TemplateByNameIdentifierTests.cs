namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.TemplateIdentifiers;

public class TemplateByNameIdentifierTests
{
    [Fact]
    public void Throws_On_Null_Argument()
    {
        this.Invoking(_ => new TemplateByNameIdentifier(name: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("name");
    }
}
