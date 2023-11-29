namespace TemplateFramework.Core.Tests.Extensions;

public class TemplateEngineExtensionsTests
{
    protected object Template { get; } = new object();
    protected IEnumerable<int> Models { get; } = [1, 2, 3];
    protected object Model { get; } = new object();

    protected const string DefaultFilename = "DefaultFilename.txt";

    public class RenderChildTemplate : TemplateEngineExtensionsTests
    {
        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Context_1(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(Model, generationEnvironment, identifier, context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_1(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            sut.RenderChildTemplate(Model, generationEnvironment, identifier, templateContext);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier));
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Context_2(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(Model, generationEnvironment, _ => identifier, context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(Model, generationEnvironment, identifierFactory: null!, templateContext))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            sut.RenderChildTemplate(Model, generationEnvironment, _ => identifier, templateContext);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier));
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Context_3(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(generationEnvironment, identifier, context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_3(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            sut.RenderChildTemplate(generationEnvironment, identifier, templateContext);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier));
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Context_4(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(generationEnvironment, () => identifier, context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_4(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(generationEnvironment, identifierFactory: null!, templateContext))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_4(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            sut.RenderChildTemplate(generationEnvironment, () => identifier, templateContext);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier));
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_5(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            sut.RenderChildTemplate(Model, generationEnvironment, identifier);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_6(
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(Model, generationEnvironment, identifierFactory: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_6(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            sut.RenderChildTemplate(Model, generationEnvironment, _ => identifier);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_7(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            sut.RenderChildTemplate(generationEnvironment, identifier);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_8(
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(generationEnvironment, identifierFactory: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_8(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            sut.RenderChildTemplate(generationEnvironment, () => identifier);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Context_9(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(Model, generationEnvironment, context: null!, identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Identifier_9(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(Model, generationEnvironment, templateContext, identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_9(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            [Frozen] ITemplateComponentRegistry templateComponentRegistry,
            [Frozen] ITemplateProvider templateProvider,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.DefaultFilename.Returns(DefaultFilename);
            templateContext.TemplateComponentRegistry.Returns(templateComponentRegistry);
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);
            templateProvider.Create(Arg.Any<ITemplateIdentifier>()).Returns(Template);

            // Act
            sut.RenderChildTemplate(Model, generationEnvironment, templateContext, identifier);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Context_10(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(generationEnvironment, context: null!, identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Throws_On_Null_Identifier_10(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplate(generationEnvironment, templateContext, identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Theory, AutoMockData]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_10(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            [Frozen] ITemplateComponentRegistry templateComponentRegistry,
            [Frozen] ITemplateProvider templateProvider,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.DefaultFilename.Returns(DefaultFilename);
            templateContext.TemplateComponentRegistry.Returns(templateComponentRegistry);
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);
            templateProvider.Create(Arg.Any<ITemplateIdentifier>()).Returns(Template);

            // Act
            sut.RenderChildTemplate(generationEnvironment, templateContext, identifier);

            // Assert
            sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier));
        }
    }

    public class RenderChildTemplates : TemplateEngineExtensionsTests
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_ChildModels_1(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(childModels: null!, generationEnvironment, identifier, templateContext))
               .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Context_1(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, identifier, context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Renders_All_Items_From_Model_1(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            sut.RenderChildTemplates(Models, generationEnvironment, identifier, templateContext);

            // Assert
            sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier));
            templateContext.Received(Models.Count()).CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_ChildModels_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(childModels: null!, generationEnvironment, _ => identifier, templateContext))
               .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Context_2(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, _ => identifier, context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Renders_All_Items_From_Model_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            sut.RenderChildTemplates(Models, generationEnvironment, _ => identifier, templateContext);

            // Assert
            sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier));
            templateContext.Received(Models.Count()).CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_ChildModels_3(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(childModels: null!, generationEnvironment, identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Theory, AutoMockData]
        public void Renders_All_Items_From_Model_3(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            sut.RenderChildTemplates(Models, generationEnvironment, identifier);

            // Assert
            sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_ChildModels_4(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(childModels: null!, generationEnvironment, _ => identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Theory, AutoMockData]
        public void Renders_All_Items_From_Model_4(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            sut.RenderChildTemplates(Models, generationEnvironment, _ => identifier);

            // Assert
            sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_ChildModels_5(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(childModels: null!, generationEnvironment, templateContext, _ => identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Context_5(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, context: null!, _ => identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_TemplateIdentifierFactory_5(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, templateContext, templateIdentifierFactory: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateIdentifierFactory");
        }

        [Theory, AutoMockData]
        public void Renders_All_Items_From_Model_5(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            [Frozen] ITemplateProvider templateProvider,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);
            templateContext.DefaultFilename.Returns(DefaultFilename);
            templateContext.TemplateComponentRegistry.Returns(templateProvider);
            templateProvider.Create(Arg.Any<ITemplateIdentifier>()).Returns(Template);

            // Act
            sut.RenderChildTemplates(Models, generationEnvironment, templateContext, _ => identifier);

            // Assert
            sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier));
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_ChildModels_6(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(childModels: null!, generationEnvironment, templateContext, _ => Template))
               .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Context_6(
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, context: null!, _ => Template))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_TemplateIdentifierFactory_6(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, templateContext, templateIdentifierFactory: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateIdentifierFactory");
        }

        [Theory, AutoMockData]
        public void Renders_All_Items_From_Model_6(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);
            templateContext.DefaultFilename.Returns(DefaultFilename);

            // Act
            sut.RenderChildTemplates(Models, generationEnvironment, templateContext, _ => Template);

            // Assert
            sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier != null));
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_ChildModels_7(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(childModels: null!, generationEnvironment, templateContext, identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Context_7(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, context: null!, identifier))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Identifier_7(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RenderChildTemplates(Models, generationEnvironment, templateContext, identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Theory, AutoMockData]
        public void Renders_All_Items_From_Model_7(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);
            templateContext.DefaultFilename.Returns(DefaultFilename);

            // Act
            sut.RenderChildTemplates(Models, generationEnvironment, templateContext, identifier);

            // Assert
            sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier));
        }
    }
}
