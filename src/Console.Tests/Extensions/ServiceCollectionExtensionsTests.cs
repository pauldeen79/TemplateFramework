namespace TemplateFramework.Console.Tests.Extensions;

public class ServiceCollectionExtensionsTests : TestBase
{
    public class AddTemplateCommands : ServiceCollectionExtensionsTests
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Arrange
            var assemblyInfoContextService = Fixture.Freeze<IAssemblyInfoContextService>();
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();

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
