namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ContextInitializerTests
{
    protected ContextInitializer CreateSut() => new();
    
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : ContextInitializerTests
    {
        [Fact]
        public void Sets_TemplateContext_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var context = new Core.TemplateContext(template);
            var request = new RenderTemplateRequest(template, new StringBuilder(), DefaultFilename, null, context);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Context.Should().BeSameAs(context);
        }

        [Fact]
        public void Initializes_New_TemplateContext_Without_Model_When_Not_Provided()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var request = new RenderTemplateRequest(template, new StringBuilder(), DefaultFilename, null);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

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
            var request = new RenderTemplateRequest(template, model, new StringBuilder(), DefaultFilename, null, context: null);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Context.Should().NotBeNull();
            template.Context.Template.Should().BeSameAs(template);
            template.Context.Model.Should().Be(model);
            template.Context.IsRootContext.Should().BeTrue();
            template.Context.ParentContext.Should().BeNull();
        }
    }
}
