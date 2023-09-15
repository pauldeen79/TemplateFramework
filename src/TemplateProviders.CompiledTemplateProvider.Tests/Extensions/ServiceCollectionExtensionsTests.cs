namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.Extensions;

public class ServiceCollectionExtensionsTests : TestBase
{
    public class AddTemplateFrameworkCompiledTemplateProvider : ServiceCollectionExtensionsTests
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved(
)
        {
            // Arrange
            var assemblyInfoContextService = Fixture.Freeze<IAssemblyInfoContextService>();
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();

            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddTemplateFrameworkCompiledTemplateProvider()
                .AddTemplateFrameworkRuntime()
                .AddSingleton(assemblyInfoContextService)
                .AddSingleton(templateFactory)
                .AddSingleton(templateComponentRegistryPluginFactory)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
