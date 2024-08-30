namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateOfStringBuilderRendererTests
{
    public class Constructor : MultipleContentTemplateOfStringBuilderRendererTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(MultipleContentOfStringBuilderTemplateRenderer).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
