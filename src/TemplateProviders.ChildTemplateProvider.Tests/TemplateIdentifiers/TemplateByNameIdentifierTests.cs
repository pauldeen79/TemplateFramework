namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.TemplateIdentifiers;

public class TemplateByNameIdentifierTests
{
    [Fact]
    public void Throws_On_Null_Arguments()
    {
        typeof(TemplateByNameIdentifier).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
    }
}
