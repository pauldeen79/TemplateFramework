namespace TemplateFramework.Core.Tests.TemplateIdentifiers;

public class TemplateInstanceIdentifierWithTemplateProviderTests
{
    public class Constructor
    {
        [Fact]
        public void Constructs_Without_Custom_CurrentDirectory()
        {
            // Act
            var instance = new TemplateInstanceIdentifierWithTemplateProvider(this, null, "assembly", "class");

            // Assert
            instance.CurrentDirectory.Should().Be(Directory.GetCurrentDirectory());
            instance.Instance.Should().BeSameAs(this);
            instance.TemplateProviderAssemblyName.Should().Be("assembly");
            instance.TemplateProviderClassName.Should().Be("class");
        }

        [Fact]
        public void Constructs_With_Custom_CurrentDirectory()
        {
            // Act
            var instance = new TemplateInstanceIdentifierWithTemplateProvider(this, "Custom", "assembly", "class");

            // Assert
            instance.CurrentDirectory.Should().Be("Custom");
            instance.Instance.Should().BeSameAs(this);
            instance.TemplateProviderAssemblyName.Should().Be("assembly");
            instance.TemplateProviderClassName.Should().Be("class");
        }
    }
}
