namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Requests;

public class CreateTemplateByModelRequestTests
{
    [Fact]
    public void Does_Not_Throw_On_Null_Argument()
    {
        this.Invoking(_ => new CreateTemplateByModelRequest(null))
            .Should().NotThrow();
    }
}
