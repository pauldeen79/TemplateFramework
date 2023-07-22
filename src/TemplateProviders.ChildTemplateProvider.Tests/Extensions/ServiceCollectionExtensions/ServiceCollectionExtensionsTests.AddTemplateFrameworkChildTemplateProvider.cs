namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFrameworkChildTemplateProvider
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddTemplateFrameworkChildTemplateProvider()
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
