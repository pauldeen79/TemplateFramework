namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Requests;

public class ChildTemplateByModelRequestTests
{
    [Fact]
    public void Does_Not_Throw_On_Null_Argument()
    {
        this.Invoking(_ => new ChildTemplateByModelRequest(null))
            .Should().NotThrow();
    }
}
