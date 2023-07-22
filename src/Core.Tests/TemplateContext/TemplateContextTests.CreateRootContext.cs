namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class CreateRootContext : TemplateContextTests
    {
        [Fact]
        public void Creates_Context_Correclty()
        {
            // Act
            var context = TemplateContext.CreateRootContext(this);

            // Assert
            context.Should().NotBeNull();
            context.Template.Should().BeSameAs(this);
            context.IsRootContext.Should().BeTrue();
        }
    }
}
