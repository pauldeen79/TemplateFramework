namespace TemplateFramework.Core.CodeGeneration.Tests.Extensions;

public class ServiceCollectionExtensionsTests : TestBase
{
    public class AddTemplateFrameworkCodeGeneration : ServiceCollectionExtensionsTests
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Arrange
            var assemblyInfoContextService = Fixture.Freeze<IAssemblyInfoContextService>();
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();

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
