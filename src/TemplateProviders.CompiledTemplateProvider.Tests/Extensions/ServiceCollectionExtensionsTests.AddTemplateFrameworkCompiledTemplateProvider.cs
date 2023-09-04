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
                .AddSingleton(new Mock<IAssemblyInfoContextService>().Object)
                .AddSingleton(new Mock<ITemplateFactory>().Object)
                .AddSingleton(new Mock<ITemplateProviderPluginFactory>().Object)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
