namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.Extensions.ServiceCollectionExtensions;

public partial class ServiceCollectionExtensionsTests
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
                .AddTransient<AddChildTemplate>()
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByModel("some string model").Should().NotBeNull();
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
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByModel("some string model").Should().NotBeNull();
        }

        [Fact]
        public void Registers_ChildTemplate_By_Name_Correctly_Without_Factory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            using var provider = services
                .AddChildTemplate<AddChildTemplate>("Name")
                .AddTransient<AddChildTemplate>()
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByName("Name").Should().NotBeNull();
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
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByName("Name").Should().NotBeNull();
        }

        [Fact]
        public void Registers_ChildTemplate_By_Name_And_ModelType_Correctly_Without_Factory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            using var provider = services
                .AddChildTemplate<AddChildTemplate>(typeof(string), "Name")
                .AddTransient<AddChildTemplate>()
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByName("Name").Should().NotBeNull();
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByModel("some string model").Should().NotBeNull();
        }

        [Fact]
        public void Registers_ChildTemplate_By_Name_And_ModelType_Correctly_With_Factory()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            using var provider = services
                .AddChildTemplate(typeof(string), "Name", _ => new AddChildTemplate())
                .BuildServiceProvider(true);
            using var scope = provider.CreateScope();

            // Assert
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByName("Name").Should().NotBeNull();
            scope.ServiceProvider.GetRequiredService<ITemplateCreator>().CreateByModel("some string model").Should().NotBeNull();
        }
    }
}
