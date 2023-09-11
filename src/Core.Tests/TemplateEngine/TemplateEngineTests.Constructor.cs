namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Constructor : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(TemplateEngine).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
