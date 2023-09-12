namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(StringBuilderTemplateRenderer).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
