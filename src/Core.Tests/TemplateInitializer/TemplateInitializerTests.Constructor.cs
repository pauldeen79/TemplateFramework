namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(TemplateInitializer).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
