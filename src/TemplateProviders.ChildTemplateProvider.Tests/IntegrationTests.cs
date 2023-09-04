﻿using System.Xml.Linq;

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
            .AddSingleton(new Mock<ITemplateProviderPluginFactory>().Object)
            .BuildServiceProvider();
        var engine = provider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine((builder, context) =>
        {
            var childTemplate = context.Provider.Create(new TemplateByNameIdentifier("MyTemplate"));
            context.Engine.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(childTemplate), builder, context.CreateChildContext(new ChildTemplateContext(new TemplateInstanceIdentifier(childTemplate)))));
        });
        var generationEnvironment = new MultipleContentBuilder();

        // Act
        engine.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment));

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }

    [Fact]
    public void Can_Render_Multiple_Files_Into_One_File_Like_Current_CsharpClassGenerator()
    {
        // Arrange
        var templateFactoryMock = new Mock<ITemplateFactory>();
        templateFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns<Type>(t => Activator.CreateInstance(t)!);
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddSingleton(templateFactoryMock.Object) // note that normally, this class needs to be implemented by the host. (like TemplateFramework.Console)
            .AddSingleton(new Mock<ITemplateProviderPluginFactory>().Object) // note that normally, this class needs to be implemented by the host. (like TemplateFramework.Console)
            .BuildServiceProvider();

        var engine = provider.GetRequiredService<ICodeGenerationEngine>();
        var templateProvider = provider.GetRequiredService<ITemplateProvider>();
        var generationEnvironment = new MultipleContentBuilderEnvironment();
        var settings = new CodeGenerationSettings(string.Empty, "GeneratedCode.cs", dryRun: true);

        // Act
        engine.Generate(new CsharpClassGeneratorCodeGenerationProvider(), templateProvider, generationEnvironment, settings);

        // Assert
        generationEnvironment.Builder.Contents.Should().HaveCount(4);
    }
}
