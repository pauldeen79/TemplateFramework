namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ContextInitializerTests
{
    protected ContextInitializerComponent CreateSut() => new();
    
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();

    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : ContextInitializerTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Sets_TemplateContext_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var context = new TemplateContext(TemplateEngineMock, TemplateProviderMock, DefaultFilename, new TemplateInstanceIdentifier(template), template);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, null, context);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Context.Should().BeSameAs(context);
        }

        [Fact]
        public void Initializes_New_TemplateContext_Without_Model_When_Not_Provided()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, null);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Context.Should().NotBeNull();
            template.Context.Template.Should().BeSameAs(template);
            template.Context.Model.Should().BeNull();
            template.Context.IsRootContext.Should().BeTrue();
            template.Context.ParentContext.Should().BeNull();
        }

        [Fact]
        public void Initializes_New_TemplateContext_With_Model_When_Not_Provided()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(ctx => ctx.Model?.ToString() ?? string.Empty); // note that this template type does not implement IModelContainer<T>, so the model property will not be set. But it will be available in the TemplateContext (untyped)
            var model = "Hello world!";
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), model, new StringBuilder(), DefaultFilename, null, context: null);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

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
