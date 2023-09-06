namespace TemplateFramework.Core.CodeGeneration.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFrameworkCodeGeneration
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddTemplateFrameworkCodeGeneration()
                .AddTemplateFrameworkRuntime()
                .AddSingleton(new Mock<IAssemblyInfoContextService>().Object)
                .AddSingleton(new Mock<ITemplateFactory>().Object)
                .AddSingleton(new Mock<ITemplateComponentRegistryPluginFactory>().Object)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
