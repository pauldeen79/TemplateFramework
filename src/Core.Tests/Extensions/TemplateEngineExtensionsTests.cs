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
        public async Task RenderChildTemplate_Throws_On_Null_Context_1(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(Model, generationEnvironment, identifier, context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_1(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            _ = await sut.RenderChildTemplate(Model, generationEnvironment, identifier, templateContext, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_Context_2(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(Model, generationEnvironment, _ => identifier, context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_IdentifierFactory_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(Model, generationEnvironment, identifierFactory: null!, templateContext, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("identifierFactory");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            _ = await sut.RenderChildTemplate(Model, generationEnvironment, _ => identifier, templateContext, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_Context_3(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(generationEnvironment, identifier, context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_3(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            _ = await sut.RenderChildTemplate(generationEnvironment, identifier, templateContext, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_Context_4(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(generationEnvironment, () => identifier, context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_IdentifierFactory_4(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(generationEnvironment, identifierFactory: null!, templateContext, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("identifierFactory");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_4(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            _ = await sut.RenderChildTemplate(generationEnvironment, () => identifier, templateContext, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
            templateContext.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_5(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            _ = await sut.RenderChildTemplate(Model, generationEnvironment, identifier, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_IdentifierFactory_6(
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(Model, generationEnvironment, identifierFactory: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("identifierFactory");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_6(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            _ = await sut.RenderChildTemplate(Model, generationEnvironment, _ => identifier, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_7(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            _ = await sut.RenderChildTemplate(generationEnvironment, identifier, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_IdentifierFactory_8(
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(generationEnvironment, identifierFactory: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("identifierFactory");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_8(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            _ = await sut.RenderChildTemplate(generationEnvironment, () => identifier, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_Context_9(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(Model, generationEnvironment, context: null!, identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_Identifier_9(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(Model, generationEnvironment, templateContext, identifier: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("identifier");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_9(
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
            _ = await sut.RenderChildTemplate(Model, generationEnvironment, templateContext, identifier, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == Model
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_Context_10(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(generationEnvironment, context: null!, identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Throws_On_Null_Identifier_10(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplate(generationEnvironment, templateContext, identifier: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("identifier");
        }

        [Theory, AutoMockData]
        public async Task RenderChildTemplate_Renders_ChildTemplate_Correctly_10(
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
            _ = await sut.RenderChildTemplate(generationEnvironment, templateContext, identifier, CancellationToken.None);

            // Assert
            await sut.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model == null
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }
    }

    public class RenderChildTemplates : TemplateEngineExtensionsTests
    {
        [Theory, AutoMockData]
        public async Task Throws_On_Null_ChildModels_1(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(childModels: null!, generationEnvironment, identifier, templateContext, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("childModels");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context_1(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, identifier, context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Renders_All_Items_From_Model_1(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            _ = await sut.RenderChildTemplates(Models, generationEnvironment, identifier, templateContext, CancellationToken.None);

            // Assert
            await sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
            templateContext.Received(Models.Count()).CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_ChildModels_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(childModels: null!, generationEnvironment, _ => identifier, templateContext, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("childModels");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context_2(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, _ => identifier, context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Renders_All_Items_From_Model_2(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);

            // Act
            _ = await sut.RenderChildTemplates(Models, generationEnvironment, _ => identifier, templateContext, CancellationToken.None);

            // Assert
            await sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
            templateContext.Received(Models.Count()).CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_ChildModels_3(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(childModels: null!, generationEnvironment, identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("childModels");
        }

        [Theory, AutoMockData]
        public async Task Renders_All_Items_From_Model_3(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            _ = await sut.RenderChildTemplates(Models, generationEnvironment, identifier, CancellationToken.None);

            // Assert
            await sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_ChildModels_4(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(childModels: null!, generationEnvironment, _ => identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("childModels");
        }

        [Theory, AutoMockData]
        public async Task Renders_All_Items_From_Model_4(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act
            _ = await sut.RenderChildTemplates(Models, generationEnvironment, _ => identifier, CancellationToken.None);

            // Assert
            await sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_ChildModels_5(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(childModels: null!, generationEnvironment, templateContext, _ => identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("childModels");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context_5(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, context: null!, _ => identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_TemplateIdentifierFactory_5(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, templateContext, templateIdentifierFactory: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("templateIdentifierFactory");
        }

        [Theory, AutoMockData]
        public async Task Renders_All_Items_From_Model_5(
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
            _ = await sut.RenderChildTemplates(Models, generationEnvironment, templateContext, _ => identifier, CancellationToken.None);

            // Assert
            await sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_ChildModels_6(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(childModels: null!, generationEnvironment, templateContext, _ => Template, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("childModels");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context_6(
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, context: null!, _ => Template, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_TemplateIdentifierFactory_6(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, templateContext, templateIdentifierFactory: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("templateIdentifierFactory");
        }

        [Theory, AutoMockData]
        public async Task Renders_All_Items_From_Model_6(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);
            templateContext.DefaultFilename.Returns(DefaultFilename);

            // Act
            _ = await sut.RenderChildTemplates(Models, generationEnvironment, templateContext, _ => Template, CancellationToken.None);

            // Assert
            await sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier != null), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_ChildModels_7(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(childModels: null!, generationEnvironment, templateContext, identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("childModels");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context_7(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, context: null!, identifier, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_Identifier_7(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            ITemplateEngine sut)
        {
            // Act & Assert
            Task t = sut.RenderChildTemplates(Models, generationEnvironment, templateContext, identifier: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("identifier");
        }

        [Theory, AutoMockData]
        public async Task Renders_All_Items_From_Model_7(
            [Frozen] ITemplateContext templateContext,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateIdentifier identifier,
            ITemplateEngine sut)
        {
            // Arrange
            templateContext.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(templateContext);
            templateContext.DefaultFilename.Returns(DefaultFilename);

            // Act
            _ = await sut.RenderChildTemplates(Models, generationEnvironment, templateContext, identifier, CancellationToken.None);

            // Assert
            await sut.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == templateContext
                && request.DefaultFilename == templateContext.DefaultFilename
                && request.GenerationEnvironment == generationEnvironment
                && request.Model != null
                && request.Model is int
                && request.Identifier == identifier), Arg.Any<CancellationToken>());
        }
    }
}
