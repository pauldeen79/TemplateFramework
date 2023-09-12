namespace TemplateFramework.Core.Tests;

public class TemplateEngineContextTests
{
    public class Constructor : TemplateEngineContextTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateEngineContext).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Theory, AutoMockData]
        public void Sets_Properties_Correctly(
            [Frozen] IRenderTemplateRequest request,
            [Frozen] ITemplateEngine engine,
            [Frozen] ITemplateComponentRegistry componentRegistry,
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier)
        {
            // Arrange
            var additionalParameters = new object();
            var model = new object();
            var template = new object();
            request.AdditionalParameters.Returns(additionalParameters);
            request.Context.Returns(templateContext);
            request.DefaultFilename.Returns("Filename.txt");
            request.GenerationEnvironment.Returns(generationEnvironment);
            request.Identifier.Returns(identifier);
            request.Model.Returns(model);

            // Act
            var instance = new TemplateEngineContext(request, engine, componentRegistry, template);

            // Assert
            instance.AdditionalParameters.Should().BeEquivalentTo(request.AdditionalParameters);
            instance.ComponentRegistry.Should().BeSameAs(componentRegistry);
            instance.Context.Should().BeSameAs(request.Context);
            instance.DefaultFilename.Should().BeSameAs(request.DefaultFilename);
            instance.Engine.Should().BeSameAs(engine);
            instance.GenerationEnvironment.Should().BeSameAs(request.GenerationEnvironment);
            instance.Identifier.Should().BeSameAs(request.Identifier);
            instance.Model.Should().BeSameAs(request.Model);
            instance.Template.Should().BeSameAs(template);
        }
    }
}
