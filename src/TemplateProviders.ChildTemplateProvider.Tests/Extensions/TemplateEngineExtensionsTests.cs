namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Extensions;

public class TemplateEngineExtensionsTests
{
    public class RenderChildTemplates
    {
        [Fact]
        public async Task Without_Context_Renders_ChildTemplates_Correctly()
        {
            // Arrange
            var templateEngine = Substitute.For<ITemplateEngine>();
            var generationEnvironment = Substitute.For<IGenerationEnvironment>();
            IEnumerable childModels = new object[] { new object(), new object(), new object() };
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await templateEngine.RenderChildTemplates(childModels, generationEnvironment, CancellationToken.None);

            // Assert
            await templateEngine.Received(3).RenderAsync(Arg.Any<IRenderTemplateRequest>(), CancellationToken.None);
        }

        [Fact]
        public async Task With_Context_Renders_ChildTemplates_Correctly()
        {
            // Arrange
            var templateEngine = Substitute.For<ITemplateEngine>();
            var generationEnvironment = Substitute.For<IGenerationEnvironment>();
            var templateContext = Substitute.For<ITemplateContext>();
            IEnumerable childModels = new object[] { new object(), new object(), new object() };
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await templateEngine.RenderChildTemplates(childModels, generationEnvironment, templateContext, CancellationToken.None);

            // Assert
            await templateEngine.Received(3).RenderAsync(Arg.Any<IRenderTemplateRequest>(), CancellationToken.None);
        }
    }

    public class RenderChildTemplate
    {
        [Fact]
        public async Task Without_Context_Renders_ChildTemplates_Correctly()
        {
            // Arrange
            var templateEngine = Substitute.For<ITemplateEngine>();
            var generationEnvironment = Substitute.For<IGenerationEnvironment>();
            object childModel = new object();

            // Act
            await templateEngine.RenderChildTemplate(childModel, generationEnvironment, CancellationToken.None);

            // Assert
            await templateEngine.Received(1).RenderAsync(Arg.Any<IRenderTemplateRequest>(), CancellationToken.None);
        }

        [Fact]
        public async Task With_Context_Renders_ChildTemplates_Correctly()
        {
            // Arrange
            var templateEngine = Substitute.For<ITemplateEngine>();
            var generationEnvironment = Substitute.For<IGenerationEnvironment>();
            var templateContext = Substitute.For<ITemplateContext>();
            object childModel = new object();

            // Act
            await templateEngine.RenderChildTemplate(childModel, generationEnvironment, templateContext, CancellationToken.None);

            // Assert
            await templateEngine.Received(1).RenderAsync(Arg.Any<IRenderTemplateRequest>(), CancellationToken.None);
        }
    }
}
