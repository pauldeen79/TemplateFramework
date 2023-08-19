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
            .BuildServiceProvider();
        var engine = provider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine((builder, context, engine, provider) =>
        {
            var childTemplate = provider.Create(new ChildTemplateByNameRequest("MyTemplate"));
            engine.Render(new RenderTemplateRequest(childTemplate, builder, context.CreateChildContext(new TemplateContext(childTemplate))));
        });
        var generationEnvironment = new MultipleContentBuilder();

        // Act
        engine.Render(new RenderTemplateRequest(template, generationEnvironment));

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }

    [Fact]
    public void Can_Render_Multiple_Files_Into_One_File_Like_Current_CsharpClassGenerator()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            // Option 1: Provide a factory (transient, unless you use the IServiceProvider argument)
            .AddChildTemplate("CodeGenerationHeader", _ => new TestData.CodeGenerationHeaderTemplate())
            // Option 2: Register the template (allow control of lifetime)
            .AddChildTemplate<TestData.DefaultUsingsTemplate>("DefaultUsings")
            .AddTransient<TestData.DefaultUsingsTemplate>()
            .AddChildTemplate<TestData.ClassTemplate>(typeof(TestData.TypeBase))
            .AddTransient<TestData.ClassTemplate>()
            .BuildServiceProvider();
        var engine = provider.GetRequiredService<ITemplateEngine>();
        var template = new TestData.BogusCsharpClassGenerator();
        var generationEnvironment = new MultipleContentBuilder();
        var model = new[]
        {
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1a" },
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1b" },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2a" },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2b" },
        };
        var additionalParameters = new Dictionary<string, object?>
        {
            { nameof(TestData.BogusCsharpClassGenerator.GenerateMultipleFiles), true },
            { nameof(TestData.BogusCsharpClassGenerator.SkipWhenFileExists), false },
            { nameof(TestData.BogusCsharpClassGenerator.CreateCodeGenerationHeader), true },
            { nameof(TestData.BogusCsharpClassGenerator.EnvironmentVersion), "1.0" },
        };

        // Act
        engine.Render(new RenderTemplateRequest(template, model, generationEnvironment, additionalParameters));

        // Assert
        generationEnvironment.Contents.Should().HaveCount(4);
    }
}
