﻿namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ContextInitializerTests
{
    public class Initialize
    {
        private const string DefaultFilename = "DefaultFilename.txt";

        [Theory, AutoMockData]
        public void Throws_On_Null_Context(ContextInitializerComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Sets_TemplateContext_On_Template_When_Possible(
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
            sut.Initialize(engineContext);

            // Assert
            template.Context.Should().BeSameAs(context);
        }

        [Theory, AutoMockData]
        public void Initializes_New_TemplateContext_Without_Model_When_Not_Provided(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            ContextInitializerComponent sut)
        {
            // Arrange
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, null);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Context.Should().NotBeNull();
            template.Context.Template.Should().BeSameAs(template);
            template.Context.Model.Should().BeNull();
            template.Context.IsRootContext.Should().BeTrue();
            template.Context.ParentContext.Should().BeNull();
        }

        [Theory, AutoMockData]
        public void Initializes_New_TemplateContext_With_Model_When_Not_Provided(
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
            sut.Initialize(engineContext);

            // Assert
            template.Context.Should().NotBeNull();
            template.Context.Template.Should().BeSameAs(template);
            template.Context.Model.Should().Be(model);
            template.Context.IsRootContext.Should().BeTrue();
            template.Context.ParentContext.Should().BeNull();
        }
    }
}
