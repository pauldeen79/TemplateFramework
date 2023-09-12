namespace TemplateFramework.Core.CodeGeneration.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFrameworkCodeGeneration
    {
        [Theory, AutoMockData]
        public void All_Dependencies_Can_Be_Resolved(
            [Frozen] IAssemblyInfoContextService assemblyInfoContextService,
            [Frozen] ITemplateFactory templateFactory,
            [Frozen] ITemplateComponentRegistryPluginFactory templateComponentRegistryPluginFactory)
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddTemplateFrameworkCodeGeneration()
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
