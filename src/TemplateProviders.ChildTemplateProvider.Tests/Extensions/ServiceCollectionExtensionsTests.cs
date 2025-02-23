namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Extensions;

public class ServiceCollectionExtensionsTests : TestBase
{
    public class AddChildTemplate
    {
        [Fact]
        public void Registers_ChildTemplate_By_ModelType_Correctly_Without_Factory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            using var provider = services
                .AddChildTemplate<AddChildTemplate>(typeof(string))
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByModel("some string model").ShouldNotBeNull();
        }

        [Fact]
        public void Registers_ChildTemplate_By_ModelType_Correctly_With_Factory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            using var provider = services
                .AddChildTemplate(typeof(string), _ => new AddChildTemplate())
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByModel("some string model").ShouldNotBeNull();
        }

        [Fact]
        public void Registers_ChildTemplate_By_Name_Correctly_Without_Factory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            using var provider = services
                .AddChildTemplate<AddChildTemplate>("Name")
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByName("Name").ShouldNotBeNull();
        }

        [Fact]
        public void Registers_ChildTemplate_By_Name_Correctly_With_Factory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            using var provider = services
                .AddChildTemplate("Name", _ => new AddChildTemplate())
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByName("Name").ShouldNotBeNull();
        }
    }

    public class AddTemplateFrameworkChildTemplateProvider : ServiceCollectionExtensionsTests
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Arrange
            var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();

            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddTemplateFrameworkChildTemplateProvider()
                .AddSingleton(templateComponentRegistryPluginFactory)
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.ShouldNotBeNull();
        }
    }
}
