namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFrameworkCompiledTemplateProvider
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddTemplateFrameworkCompiledTemplateProvider()
                .AddTemplateFrameworkRuntime()
                .AddSingleton(Substitute.For<IAssemblyInfoContextService>())
                .AddSingleton(Substitute.For<ITemplateFactory>())
                .AddSingleton(Substitute.For<ITemplateComponentRegistryPluginFactory>())
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
