namespace TemplateFramework.Core.Tests.TemplateIdentifiers;

public class TemplateInstanceIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateInstanceIdentifier).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Fact]
        public void Sets_Instance_Correctly()
        {
            // Act
            var identifier = new TemplateInstanceIdentifier(this);

            // Assert
            identifier.Instance.ShouldBeSameAs(this);
        }
    }
}
