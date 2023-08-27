namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Requests;

public class ChildTemplateByNameRequestTests
{
    [Fact]
    public void Throws_On_Null_Argument()
    {
        this.Invoking(_ => new CreateChildTemplateByNameRequest(name: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("name");
    }
}
