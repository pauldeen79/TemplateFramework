namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_Template_From_CompiledTemplateProvider()
    {
        // Arrange
        var templateFactoryMock = Substitute.For<ITemplateFactory>();
        templateFactoryMock.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkRuntime()
            .AddTemplateFrameworkCompiledTemplateProvider()
            .AddSingleton(Substitute.For<IAssemblyInfoContextService>())
            .AddSingleton(templateFactoryMock)
            .AddSingleton(Substitute.For<ITemplateComponentRegistryPluginFactory>())
            .BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var templateProvider = scope.ServiceProvider.GetRequiredService<ITemplateProvider>();
        var template = templateProvider.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, typeof(MyTemplate).FullName!));
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var builder = new StringBuilder();
        var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), builder);

        // Act
        templateEngine.Render(request);

        // Assert
        builder.ToString().Should().Be("Hello world!");
    }
}

public sealed class MyTemplate
{
    public override string ToString() => "Hello world!";
}
