namespace TemplateFramework.Core.Tests.Extensions;

public partial class ServiceCollectionExtensionsTests
{
    public class AddTemplateFramework
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddSingleton(Substitute.For<ITemplateComponentRegistryPluginFactory>())
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
