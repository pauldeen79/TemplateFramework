namespace TemplateFramework.Core.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_Template()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddSingleton(Substitute.For<ITemplateComponentRegistryPluginFactory>())
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.Template(builder => builder.Append("Hello world!"));
        var builder = new MultipleContentBuilder();

        // Act
        sut.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), builder));

        // Assert
        builder.Contents.Should().ContainSingle();
        builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
    }
}
