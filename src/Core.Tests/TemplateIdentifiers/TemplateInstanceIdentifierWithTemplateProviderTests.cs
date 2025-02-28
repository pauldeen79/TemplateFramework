namespace TemplateFramework.Core.Tests.TemplateIdentifiers;

public class TemplateInstanceIdentifierWithTemplateProviderTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateInstanceIdentifierWithTemplateProvider).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(p => !new[] { "currentDirectory", "templateProviderAssemblyName", "templateProviderClassName" }.Contains(p.Name));
        }

        [Fact]
        public void Constructs_Without_Custom_CurrentDirectory()
        {
            // Act
            var instance = new TemplateInstanceIdentifierWithTemplateProvider(this, null, "assembly", "class");

            // Assert
            instance.CurrentDirectory.ShouldBe(Directory.GetCurrentDirectory());
            instance.Instance.ShouldBeSameAs(this);
            instance.PluginAssemblyName.ShouldBe("assembly");
            instance.PluginClassName.ShouldBe("class");
        }

        [Fact]
        public void Constructs_With_Custom_CurrentDirectory()
        {
            // Act
            var instance = new TemplateInstanceIdentifierWithTemplateProvider(this, "Custom", "assembly", "class");

            // Assert
            instance.CurrentDirectory.ShouldBe("Custom");
            instance.Instance.ShouldBeSameAs(this);
            instance.PluginAssemblyName.ShouldBe("assembly");
            instance.PluginClassName.ShouldBe("class");
        }
    }
}
