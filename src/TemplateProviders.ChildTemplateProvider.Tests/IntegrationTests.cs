namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_MultipleContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddChildTemplate("MyTemplate", _ => new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext))
            .AddSingleton(Substitute.For<ITemplateComponentRegistryPluginFactory>())
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine((builder, context) =>
        {
            var identifier = new TemplateByNameIdentifier("MyTemplate");
            context.Engine.RenderChildTemplate(new MultipleContentBuilderEnvironment(builder), identifier, context);
        });
        var generationEnvironment = new MultipleContentBuilder();

        // Act
        engine.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment));

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }

    [Theory, AutoMockData]
    public void Can_Render_Multiple_Files_Into_One_File_Like_Current_CsharpClassGenerator(
        [Frozen] ITemplateFactory templateFactory,
        [Frozen] ITemplateComponentRegistryPluginFactory templateComponentRegistryPluginFactory)
    {
        // Arrange
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
        var generationEnvironment = new MultipleContentBuilderEnvironment();
        var settings = new CodeGenerationSettings(string.Empty, "GeneratedCode.cs", dryRun: true);

        // Act
        engine.Generate(new CsharpClassGeneratorCodeGenerationProvider(), generationEnvironment, settings);

        // Assert
        generationEnvironment.Builder.Contents.Should().HaveCount(4);
    }
}
