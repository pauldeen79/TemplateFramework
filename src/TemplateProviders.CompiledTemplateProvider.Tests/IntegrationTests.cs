namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_Template_From_CompiledTemplateProvider()
    {
        // Arrange
        var templateFactoryMock = new Mock<ITemplateFactory>();
        templateFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns<Type>(t => Activator.CreateInstance(t)!);
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkRuntime()
            .AddTemplateFrameworkCompiledTemplateProvider()
            .AddSingleton(new Mock<IAssemblyInfoContextService>().Object)
            .AddSingleton(templateFactoryMock.Object)
            .AddSingleton(new Mock<ITemplateProviderPluginFactory>().Object)
            .BuildServiceProvider();
        var templateProvider = provider.GetRequiredService<ITemplateProvider>();
        var template = templateProvider.Create(new CreateCompiledTemplateRequest(GetType().Assembly.FullName!, typeof(MyTemplate).FullName!));
        var templateEngine = provider.GetRequiredService<ITemplateEngine>();
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
