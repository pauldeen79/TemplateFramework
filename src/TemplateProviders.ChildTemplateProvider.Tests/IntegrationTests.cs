namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public async Task Can_Render_MultipleStringContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
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
            await context.Engine.RenderChildTemplate(new MultipleStringContentBuilderEnvironment(builder), identifier, context, CancellationToken.None).ConfigureAwait(false);
        });
        var generationEnvironment = new MultipleContentBuilder();

        // Act
        await engine.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment), CancellationToken.None);

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }

    [Fact]
    public async Task Can_Render_MultipleStringContentBuilderTemplate_With_ChildTemplate_Containing_ViewModel_And_TemplateContext()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddChildTemplate(typeof(TestData.Model), _ => new TestData.TemplateWithViewModel<TestData.ViewModel<TestData.Model>>((builder, viewModel) => builder.Append(viewModel?.Model?.Contents ?? string.Empty)))
            .AddViewModel<TestData.ViewModel<TestData.Model>>()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(async (builder, context) =>
        {
            var model = new TestData.Model { Contents = "Hello world!" };
            await context.Engine.RenderChildTemplate(model, new MultipleStringContentBuilderEnvironment(builder), context, CancellationToken.None).ConfigureAwait(false);
        });
        var generationEnvironment = new MultipleContentBuilder();

        // Act
        await engine.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment), CancellationToken.None);

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Hello world!");
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
        var generationEnvironment = new MultipleStringContentBuilderEnvironment();
        var settings = new CodeGenerationSettings(string.Empty, "GeneratedCode.cs", dryRun: true);

        // Act
        await engine.Generate(new CsharpClassGeneratorCodeGenerationProvider(), generationEnvironment, settings);

        // Assert
        generationEnvironment.Builder.Contents.Should().HaveCount(4);
    }

    [Fact]
    public async Task Can_Render_Template_To_Single_XDocument()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddScoped<ITemplateRenderer, XDocumentBuilderTemplateRenderer>()
            .AddChildTemplate<XDocumentTemplate>("XDocumentTemplate")
            .AddChildTemplate<SubItemTemplate>("SubItem")
            .AddViewModel<TestData.ViewModel<TestData.Model>>()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var generationEnvironment = new XDocumentGenerationEnvironment(new XElement("MyRootElement"));
        var model = new XDocumentTestModel("Item1", "Item2", "Item3");

        // Act
        await engine.Render(new RenderTemplateRequest(new TemplateByNameIdentifier("XDocumentTemplate"), model, generationEnvironment, string.Empty, null, null), CancellationToken.None);

        // Assert
        generationEnvironment.Builder.Document.ToString().Should().Be(@"<MyRootElement processed=""true"">
  <subItems>
    <item>Item1</item>
    <item>Item2</item>
    <item>Item3</item>
  </subItems>
</MyRootElement>");
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
