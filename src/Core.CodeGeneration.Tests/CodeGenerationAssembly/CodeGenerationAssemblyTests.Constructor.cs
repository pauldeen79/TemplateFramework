namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    public class Constructor : CodeGenerationAssemblyTests
    {
        [Fact]
        public void Throws_On_Null_CodeGenerationEngine()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(codeGenerationEngine: null!, assemblyService: AssemblyServiceMock, creators: Enumerable.Empty<ICodeGenerationProviderCreator>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("codeGenerationEngine");
        }

        [Fact]
        public void Throws_On_Null_AssemblyService()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(codeGenerationEngine: CodeGenerationEngineMock, assemblyService: null!, creators: Enumerable.Empty<ICodeGenerationProviderCreator>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("assemblyService");
        }

        [Fact]
        public void Throws_On_Null_Creators()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(codeGenerationEngine: CodeGenerationEngineMock, assemblyService: AssemblyServiceMock, creators: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("creators");
        }
    }
}
