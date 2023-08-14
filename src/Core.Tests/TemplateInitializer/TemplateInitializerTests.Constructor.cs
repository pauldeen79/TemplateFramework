namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Components()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateInitializer(components: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("components");
        }
    }
}
