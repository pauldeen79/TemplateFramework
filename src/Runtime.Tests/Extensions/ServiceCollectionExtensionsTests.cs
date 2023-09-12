namespace TemplateFramework.Runtime.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFrameworkRuntime
    {
        [Theory, AutoMockData]
        public void All_Dependencies_Can_Be_Resolved([Frozen] IAssemblyInfoContextService assemblyInfoContextService)
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFrameworkRuntime()
                .AddSingleton(assemblyInfoContextService)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
