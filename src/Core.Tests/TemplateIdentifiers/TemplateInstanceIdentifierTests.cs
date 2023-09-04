namespace TemplateFramework.Core.Tests.TemplateIdentifiers;

public class TemplateInstanceIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Instance()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateInstanceIdentifier(instance: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("instance");
        }

        [Fact]
        public void Sets_Instance_Correctly()
        {
            // Act
            var identifier = new TemplateInstanceIdentifier(this);

            // Assert
            identifier.Instance.Should().BeSameAs(this);
        }
    }
}
