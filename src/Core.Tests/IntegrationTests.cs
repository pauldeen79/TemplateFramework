namespace TemplateFramework.Core.Tests;

public class IntegrationTests
{
    [Theory, AutoMockData]
    public async Task Can_Render_Template([Frozen] ITemplateComponentRegistryPluginFactory templateComponentRegistryPluginFactory)
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.Template(builder => builder.Append("Hello world!"));
        var builder = new MultipleContentBuilder();

        // Act
        await sut.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), builder), CancellationToken.None);

        // Assert
        builder.Contents.Count().ShouldBe(1);
        builder.Contents.Single().Builder.ToString().ShouldBe("Hello world!");
    }
}
