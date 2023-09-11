namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Constructor : MultipleContentTemplateRendererTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(MultipleContentTemplateRenderer).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
