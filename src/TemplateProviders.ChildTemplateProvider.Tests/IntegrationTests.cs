namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public async Task Can_Render_MultipleContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddChildTemplate("MyTemplate", _ => new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext))
            .AddSingleton(templateComponentRegistryPluginFactory)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(async (builder, context) =>
        {
            var identifier = new TemplateByNameIdentifier("MyTemplate");
            await context.Engine.RenderChildTemplate(new MultipleContentBuilderEnvironment<StringBuilder>(builder), identifier, context, CancellationToken.None).ConfigureAwait(false);
        });
        var generationEnvironment = new MultipleContentBuilder();

        // Act
        await engine.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment), CancellationToken.None);

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }

    [Fact]
    public async Task Can_Render_Multiple_Files_Into_One_File_Like_Current_CsharpClassGenerator()
    {
        // Arrange
        var templateFactory = Fixture.Freeze<ITemplateFactory>();
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        templateFactory.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddSingleton(templateFactory) // note that normally, this class needs to be implemented by the host. (like TemplateFramework.Console)
            .AddSingleton(templateComponentRegistryPluginFactory) // note that normally, this class needs to be implemented by the host. (like TemplateFramework.Console)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        var engine = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var generationEnvironment = new MultipleContentBuilderEnvironment<StringBuilder>();
        var settings = new CodeGenerationSettings(string.Empty, "GeneratedCode.cs", dryRun: true);

        // Act
        await engine.Generate(new CsharpClassGeneratorCodeGenerationProvider(), generationEnvironment, settings);

        // Assert
        generationEnvironment.Builder.Contents.Should().HaveCount(4);
    }

    [Fact]
    public void Rendering_Unknown_Template_By_Name_Gives_Clear_ErrorMessage_What_Is_Wrong()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddChildTemplate("MyTemplate", _ => new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext))
            .AddSingleton(templateComponentRegistryPluginFactory)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var generationEnvironment = new MultipleContentBuilder();

        // Act & Assert
        engine.Awaiting(x => x.Render(new RenderTemplateRequest(new TemplateByNameIdentifier("Unknown"), generationEnvironment), CancellationToken.None))
              .Should().ThrowAsync<NotSupportedException>().WithMessage("Template with name Unknown is not supported");
    }

    [Fact]
    public void Rendering_Unknown_Template_By_Model_Gives_Clear_ErrorMessage_What_Is_Wrong()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddChildTemplate("MyTemplate", _ => new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext))
            .AddSingleton(templateComponentRegistryPluginFactory)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var generationEnvironment = new MultipleContentBuilder();

        // Act & Assert
        engine.Awaiting(x => x.Render(new RenderTemplateRequest(new TemplateByModelIdentifier("Unknown"), generationEnvironment), CancellationToken.None))
              .Should().ThrowAsync<NotSupportedException>().WithMessage("Model of type System.String is not supported");
    }
}
