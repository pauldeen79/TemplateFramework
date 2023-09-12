namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.TemplateIdentifiers;

public class TemplateByModelIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Does_Not_Throw_On_Null_Argument()
        {
            this.Invoking(_ => new TemplateByModelIdentifier(null))
                .Should().NotThrow();
        }
    }
}
