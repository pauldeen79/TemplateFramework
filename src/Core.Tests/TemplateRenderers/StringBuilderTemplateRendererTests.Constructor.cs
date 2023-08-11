namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Renderers()
        {
            // Act & Assert
            this.Invoking(_ => new StringBuilderTemplateRenderer(renderers: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("renderers");
        }
    }
}
