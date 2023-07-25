namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationEngineTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_TemplateEngine()
        {
            this.Invoking(_ => new CodeGenerationEngine(templateEngine: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateEngine");
        }
    }
}
