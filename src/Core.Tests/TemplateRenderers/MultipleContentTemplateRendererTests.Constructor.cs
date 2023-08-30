namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Constructor : MultipleContentTemplateRendererTests
    {
        [Fact]
        public void Throws_On_Null_SingleContentTemplateRenderer()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentTemplateRenderer(singleContentTemplateRenderer: null!, TemplateProviderMock.Object, creators: Enumerable.Empty<IMultipleContentBuilderTemplateCreator>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("singleContentTemplateRenderer");
        }

        [Fact]
        public void Throws_On_Null_Provider()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentTemplateRenderer(SingleContentTemplateRendererMock.Object, provider: null!, creators: Enumerable.Empty<IMultipleContentBuilderTemplateCreator>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Null_Creators()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentTemplateRenderer(SingleContentTemplateRendererMock.Object, TemplateProviderMock.Object, creators: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("creators");
        }
    }
}
