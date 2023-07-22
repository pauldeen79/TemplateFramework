namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class UnsupportedGenerationEnvironmentType
    {
        [Fact]
        public void Throws()
        {
            // Arrange
            var sut = new TemplateEngine(new DefaultTemplateInitializer(Enumerable.Empty<ITemplateParameterConverter>()), Array.Empty<ITemplateRenderer>()); // we are specifying here that no renderers are known, so even StringBuilder throws an exception :)
            var request = new RenderTemplateRequest(new TestData.Template(_ => { }), new StringBuilder()); // note that we can't put a non-supported type in here because the interface prevents that. But the construction above accomplishes that.

            // Act & Assert
            sut.Invoking(x => x.Render(request))
               .Should().Throw<NotSupportedException>();
        }
    }
}
