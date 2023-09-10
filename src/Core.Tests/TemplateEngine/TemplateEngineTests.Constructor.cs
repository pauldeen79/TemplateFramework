namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Constructor : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Provider()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(provider: null!, TemplateInitializerMock, TemplateParameterExtractorMock, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Null_Initializer()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateProviderMock, initializer: null!, TemplateParameterExtractorMock, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("initializer");
        }

        [Fact]
        public void Throws_On_Null_ParameterExtractor()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateProviderMock, TemplateInitializerMock, parameterExtractor: null!, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("parameterExtractor");
        }

        [Fact]
        public void Throws_On_Null_Renderers()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateProviderMock, TemplateInitializerMock, TemplateParameterExtractorMock, renderers: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("renderers");
        }
    }
}
