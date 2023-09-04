namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationEngineTests
{
    public class Constructor : CodeGenerationEngineTests
    {
        [Fact]
        public void Throws_On_Null_TemplateEngine()
        {
            this.Invoking(_ => new CodeGenerationEngine(templateEngine: null!, TemplateFactoryMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateEngine");
        }

        [Fact]
        public void Throws_On_Null_TemplateFactory()
        {
            this.Invoking(_ => new CodeGenerationEngine(TemplateEngineMock.Object, templateFactory: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }
    }
}
