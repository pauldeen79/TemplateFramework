namespace TemplateFramework.Runtime.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFrameworkRuntime
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFrameworkRuntime()
                .AddSingleton(Substitute.For<IAssemblyInfoContextService>())
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
