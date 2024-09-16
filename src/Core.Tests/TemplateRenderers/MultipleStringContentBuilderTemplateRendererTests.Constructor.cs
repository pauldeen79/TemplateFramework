namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleStringContentBuilderTemplateRendererTests
{
    public class Constructor : MultipleStringContentBuilderTemplateRendererTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(MultipleStringContentBuilderTemplateRenderer).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
