namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ContextInitializerComponentTests
{
    public class Initialize
    {
        private const string DefaultFilename = "DefaultFilename.txt";

        [Theory, AutoMockData]
        public void Throws_On_Null_Context(ContextInitializerComponent sut)
        {
            // Act & Assert
            Action a = () => sut.InitializeAsync(context: null!, CancellationToken.None);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Sets_TemplateContext_On_Template_When_Possible(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            ContextInitializerComponent sut)
        {
            // Arrange
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var context = new TemplateContext(templateEngine, templateProvider, DefaultFilename, new TemplateInstanceIdentifier(template), template);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, null, context);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.Context.ShouldBeSameAs(context);
        }

        [Theory, AutoMockData]
        public async Task Initializes_New_TemplateContext_Without_Model_When_Not_Provided(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            ContextInitializerComponent sut)
        {
            // Arrange
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, null);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.Context.ShouldNotBeNull();
            template.Context.Template.ShouldBeSameAs(template);
            template.Context.Model.ShouldBeNull();
            template.Context.IsRootContext.ShouldBeTrue();
            template.Context.ParentContext.ShouldBeNull();
        }

        [Theory, AutoMockData]
        public async Task Initializes_New_TemplateContext_With_Model_When_Not_Provided(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            ContextInitializerComponent sut)
        {
            // Arrange
            var template = new TestData.PlainTemplateWithTemplateContext(ctx => ctx.Model?.ToString() ?? string.Empty); // note that this template type does not implement IModelContainer<T>, so the model property will not be set. But it will be available in the TemplateContext (untyped)
            var model = "Hello world!";
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), model, new StringBuilder(), DefaultFilename, null, context: null);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.Context.ShouldNotBeNull();
            template.Context.Template.ShouldBeSameAs(template);
            template.Context.Model.ShouldBe(model);
            template.Context.IsRootContext.ShouldBeTrue();
            template.Context.ParentContext.ShouldBeNull();
        }
    }
}
