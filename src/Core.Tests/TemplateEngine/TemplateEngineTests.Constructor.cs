namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Constructor : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Provider()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(provider: null!, TemplateInitializerMock.Object, TemplateParameterExtractorMock.Object, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Null_Initializer()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateProviderMock.Object, initializer: null!, TemplateParameterExtractorMock.Object, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("initializer");
        }

        [Fact]
        public void Throws_On_Null_ParameterExtractor()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateProviderMock.Object, TemplateInitializerMock.Object, parameterExtractor: null!, Enumerable.Empty<ITemplateRenderer>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("parameterExtractor");
        }

        [Fact]
        public void Throws_On_Null_Renderers()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngine(TemplateProviderMock.Object, TemplateInitializerMock.Object, TemplateParameterExtractorMock.Object, renderers: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("renderers");
        }
    }
}
