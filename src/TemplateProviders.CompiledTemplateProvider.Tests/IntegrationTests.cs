namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public async Task Can_Render_Template_From_CompiledTemplateProvider()
    {
        // Arrange
        var assemblyInfoContextService = Fixture.Freeze<IAssemblyInfoContextService>();
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkRuntime()
            .AddTemplateFrameworkCompiledTemplateProvider()
            .AddSingleton(assemblyInfoContextService)
            .AddSingleton(templateComponentRegistryPluginFactory)
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var templateProvider = scope.ServiceProvider.GetRequiredService<ITemplateProvider>();
        var template = templateProvider.Create(new TemplateInstanceIdentifier(new MyTemplate()));
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var builder = new StringBuilder();
        var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), builder);

        // Act
        await templateEngine.RenderAsync(request, CancellationToken.None);

        // Assert
        builder.ToString().ShouldBe("Hello world!");
    }
}

public sealed class MyTemplate
{
    public override string ToString() => "Hello world!";
}
