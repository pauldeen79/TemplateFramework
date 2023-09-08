namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Act
        using var provider = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(new Mock<ITemplateComponentRegistryPluginFactory>().Object)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        // Assert
        provider.Should().NotBeNull();
    }
}
