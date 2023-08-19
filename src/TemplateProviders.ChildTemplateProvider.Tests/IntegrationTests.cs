using System.Data;
using TemplateFramework.Abstractions;

namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public class IntegrationTests
{
    private readonly Mock<ITemplateCreator> _templateCreatorMock;

    public IntegrationTests()
    {
        _templateCreatorMock = new Mock<ITemplateCreator>();
        _templateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>()))
                            .Returns<string>(name => name == "MyTemplate");
        _templateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>()))
                            .Returns<string>(name => name == "MyTemplate"
                                ? new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext)
                                : throw new NotSupportedException("What are you doing?!"));
    }

    [Fact]
    public void Can_Render_MultipleContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddSingleton(_ => _templateCreatorMock.Object)
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
            .AddChildTemplate<TestData.CodeGenerationHeaderTemplate>("CodeGenerationHeader")
            .AddChildTemplate<TestData.DefaultUsingsTemplate>("DefaultUsings")
            .AddChildTemplate<TestData.ClassTemplate>(typeof(TestData.TypeBase))
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

        // Act
        engine.Render(new RenderTemplateRequest(template, model, generationEnvironment, new { GenerateMultipleFiles = true, SkipWhenFileExists = false, CreateCodeGenerationHeader = true }));

        // Assert
        generationEnvironment.Contents.Should().HaveCount(4);
    }
}
