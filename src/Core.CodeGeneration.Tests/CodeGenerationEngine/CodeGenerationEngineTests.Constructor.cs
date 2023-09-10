namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationEngineTests
{
    public class Constructor : CodeGenerationEngineTests
    {
        [Fact]
        public void Throws_On_Null_TemplateEngine()
        {
            this.Invoking(_ => new CodeGenerationEngine(templateEngine: null!, TemplateFactoryMock, TemplateProviderMock))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateEngine");
        }

        [Fact]
        public void Throws_On_Null_TemplateFactory()
        {
            this.Invoking(_ => new CodeGenerationEngine(TemplateEngineMock, templateFactory: null!, TemplateProviderMock))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void Throws_On_Null_TemplateProvider()
        {
            this.Invoking(_ => new CodeGenerationEngine(TemplateEngineMock, TemplateFactoryMock, templateProvider: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateProvider");
        }
    }
}
