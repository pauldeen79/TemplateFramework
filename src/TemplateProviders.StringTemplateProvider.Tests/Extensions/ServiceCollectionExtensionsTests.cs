namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests.Extensions;

public class ServiceCollectionExtensionsTests : TestBase
{
    public class AddTemplateFrameworkStringTemplateProvider : ServiceCollectionExtensionsTests
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Arrange
            var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();

            // Act
            using var provider = new ServiceCollection()
                .AddExpressionEvaluator()
                .AddTemplateFramework()
                .AddTemplateFrameworkStringTemplateProvider()
                .AddSingleton(templateComponentRegistryPluginFactory)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.ShouldNotBeNull();
        }
    }
}
