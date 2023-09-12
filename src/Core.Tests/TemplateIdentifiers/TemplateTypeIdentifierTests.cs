namespace TemplateFramework.Core.Tests.TemplateIdentifiers;

public class TemplateTypeIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateTypeIdentifier).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
