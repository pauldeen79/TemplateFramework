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

        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine((builder, context) =>
        {
            var childTemplate = context.Provider.Create(new ChildTemplateByNameRequest("MyTemplate"));
            context.Engine.Render(new RenderTemplateRequest(childTemplate, builder, context.CreateChildContext(new TemplateContext(context.Engine, context.Provider, "Filename.txt", childTemplate))));
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
            .AddChildTemplate("CodeGenerationHeader", _ => new TestData.CodeGenerationHeaderTemplate())
            .AddChildTemplate("DefaultUsings", _ => new TestData.DefaultUsingsTemplate())
            .AddChildTemplate(typeof(TestData.TypeBase), _ => new TestData.ClassTemplate())
            .BuildServiceProvider();
        var engine = provider.GetRequiredService<ITemplateEngine>();
        var template = new TestData.CsharpClassGenerator();
        var generationEnvironment = new MultipleContentBuilder();
        var model = new[]
        {
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1a", Usings = new[] { "ModelFramework" } },
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1b", Usings = new[] { "ModelFramework", "ModelFramework.Domain" } },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2a" },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2b", SubClasses = new[] { new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass1" }, new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass2", SubClasses = new[] { new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass2a" } } } } },
        };
        var settings = new TestData.CsharpClassGeneratorSettings
        (
            generateMultipleFiles: true,
            skipWhenFileExists: false,
            createCodeGenerationHeader: true,
            environmentVersion: "1.0",
            filenamePrefix: "Entities/",
            filenameSuffix: ".generated",
            enableNullableContext: true,
            indentCount: 1,
            cultureInfo: CultureInfo.CurrentCulture
        );
        var viewModel = new TestData.CsharpClassGeneratorViewModel<IEnumerable<TestData.TypeBase>>(model, settings);

        // Act
        engine.Render(new RenderTemplateRequest(template, viewModel, generationEnvironment, settings));

        // Assert
        generationEnvironment.Contents.Should().HaveCount(4);
    }
}
