namespace TemplateFramework.Core.Tests;

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
            .AddSingleton(_ => _templateCreatorMock.Object)
            .BuildServiceProvider();
        var sut = provider.GetRequiredService<ITemplateEngine>();

        var templateFactory = provider.GetRequiredService<ITemplateFactory>();
        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(templateFactory, (builder, context, engine, factory) =>
        {
            var childTemplate = factory.CreateByName("MyTemplate");
            engine.Render(new RenderTemplateRequest(childTemplate, builder, context.CreateChildContext(childTemplate)));
        });
        var fileSystemMock = new Mock<IFileSystem>();
        var generationEnvironment = new MultipleContentBuilder(fileSystemMock.Object, Encoding.UTF8, TestData.BasePath);

        // Act
        sut.Render(new RenderTemplateRequest(template, generationEnvironment));

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }
}
