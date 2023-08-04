namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Creators()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentTemplateRenderer(creators: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("creators");
        }
    }
}
