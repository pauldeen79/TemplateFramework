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
        var sut = provider.GetRequiredService<ITemplateEngine>();

        var templateProvider = provider.GetRequiredService<ITemplateProvider>();
        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(templateProvider, (builder, context, engine, provider) =>
        {
            var childTemplate = provider.Create(new ChildTemplateByNameRequest("MyTemplate"));
            engine.Render(new RenderTemplateRequest(childTemplate, builder, context.CreateChildContext(new TemplateContext(childTemplate))));
        });
        var generationEnvironment = new MultipleContentBuilder(TestData.BasePath);

        // Act
        sut.Render(new RenderTemplateRequest(template, generationEnvironment));

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }
}
