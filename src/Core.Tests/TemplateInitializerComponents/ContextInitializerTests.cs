namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ContextInitializerTests
{
    protected ContextInitializer CreateSut() => new(TemplateProviderMock.Object);
    
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : ContextInitializerTests
    {
        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(request: null!, TemplateEngineMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_On_Null_Engine()
        {
            // Arrange
            var sut = CreateSut();
            var template = this;
            var request = new RenderTemplateRequest(template, null, new StringBuilder(), DefaultFilename);

            // Act & Assert
            sut.Invoking(x => x.Initialize(request, engine: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("engine");
        }

        [Fact]
        public void Sets_TemplateContext_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var context = new TemplateContext(TemplateEngineMock.Object, TemplateProviderMock.Object, DefaultFilename, template);
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
