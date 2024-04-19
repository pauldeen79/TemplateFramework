namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class UnsupportedGenerationEnvironmentType : TemplateEngineTests
    {
        [Fact]
        public void Throws()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateProviderMock, TemplateInitializerMock, TemplateParameterExtractorMock, Array.Empty<ITemplateRenderer>()); // we are specifying here that no renderers are known, so even StringBuilder throws an exception :)
            var template = new TestData.Template(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder()); // note that we can't put a non-supported type in here because the interface prevents that. But the construction above accomplishes that.
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);

            // Act & Assert
            sut.Invoking(x => x.Render(request))
               .Should().Throw<NotSupportedException>();
        }
    }
}
