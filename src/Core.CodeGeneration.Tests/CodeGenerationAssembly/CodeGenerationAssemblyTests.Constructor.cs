namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    public class Constructor : CodeGenerationAssemblyTests
    {
        [Fact]
        public void Throws_On_Null_CodeGenerationEngine()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(codeGenerationEngine: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("codeGenerationEngine");
        }
    }
}
