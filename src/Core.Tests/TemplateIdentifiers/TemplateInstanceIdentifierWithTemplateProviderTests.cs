namespace TemplateFramework.Core.Tests.TemplateIdentifiers;

public class TemplateInstanceIdentifierWithTemplateProviderTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(TemplateInstanceIdentifierWithTemplateProvider).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(p => !new[] { "currentDirectory", "templateProviderAssemblyName", "templateProviderClassName" }.Contains(p.Name));
        }

        [Fact]
        public void Constructs_Without_Custom_CurrentDirectory()
        {
            // Act
            var instance = new TemplateInstanceIdentifierWithTemplateProvider(this, null, "assembly", "class");

            // Assert
            instance.CurrentDirectory.Should().Be(Directory.GetCurrentDirectory());
            instance.Instance.Should().BeSameAs(this);
            instance.PluginAssemblyName.Should().Be("assembly");
            instance.PluginClassName.Should().Be("class");
        }

        [Fact]
        public void Constructs_With_Custom_CurrentDirectory()
        {
            // Act
            var instance = new TemplateInstanceIdentifierWithTemplateProvider(this, "Custom", "assembly", "class");

            // Assert
            instance.CurrentDirectory.Should().Be("Custom");
            instance.Instance.Should().BeSameAs(this);
            instance.PluginAssemblyName.Should().Be("assembly");
            instance.PluginClassName.Should().Be("class");
        }
    }
}
