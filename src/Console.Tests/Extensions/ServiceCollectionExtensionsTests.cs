namespace TemplateFramework.Console.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateCommands
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            var services = new ServiceCollection();
            services.InjectClipboard();
            using var provider = services
                .AddTemplateFramework()
                .AddTemplateFrameworkCodeGeneration()
                .AddTemplateFrameworkRuntime()
                .AddTemplateCommands()
                .AddSingleton(Substitute.For<IAssemblyInfoContextService>())
                .AddSingleton(Substitute.For<ITemplateFactory>())
                .AddSingleton(Substitute.For<ITemplateComponentRegistryPluginFactory>())
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
