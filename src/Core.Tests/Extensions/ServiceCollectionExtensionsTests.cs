namespace TemplateFramework.Core.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFramework
    {
        [Theory, AutoMockData]
        public void All_Dependencies_Can_Be_Resolved([Frozen] ITemplateComponentRegistryPluginFactory templateComponentRegistryPluginFactory)
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddSingleton(templateComponentRegistryPluginFactory)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
