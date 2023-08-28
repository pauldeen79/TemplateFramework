namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Constructor : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_TemplateInitializer()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(templateInitializer: null!, TemplateParameterExtractorMock.Object, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateInitializer");
        }

        [Fact]
        public void Throws_On_Null_TemplateParameterExtractor()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateInitializerMock.Object, templateParameterExtractor: null!, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateParameterExtractor");
        }

        [Fact]
        public void Throws_On_Null_TemplateRenderers()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateInitializerMock.Object, TemplateParameterExtractorMock.Object, templateRenderers: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateRenderers");
        }
    }
}
