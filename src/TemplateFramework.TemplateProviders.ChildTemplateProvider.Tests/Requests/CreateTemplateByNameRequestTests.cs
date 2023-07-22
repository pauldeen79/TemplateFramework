namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Requests;

public class CreateTemplateByNameRequestTests
{
    [Fact]
    public void Throws_On_Null_Argument()
    {
        this.Invoking(_ => new CreateTemplateByNameRequest(name: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("name");
    }
}
