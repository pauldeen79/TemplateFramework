namespace TemplateFramework.Console.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateCommands
    {
        [Theory, AutoMockData]
        public void All_Dependencies_Can_Be_Resolved(
            [Frozen] IAssemblyInfoContextService assemblyInfoContextService,
            [Frozen] ITemplateFactory templateFactory,
            [Frozen] ITemplateComponentRegistryPluginFactory templateComponentRegistryPluginFactory)
        {
            // Act
            var services = new ServiceCollection();
            services.InjectClipboard();
            using var provider = services
                .AddTemplateFramework()
                .AddTemplateFrameworkCodeGeneration()
                .AddTemplateFrameworkRuntime()
                .AddTemplateCommands()
                .AddSingleton(assemblyInfoContextService)
                .AddSingleton(templateFactory)
                .AddSingleton(templateComponentRegistryPluginFactory)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
