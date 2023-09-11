namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests.TemplateIdentifiers;

public class FormattableStringTemplateIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(FormattableStringTemplateIdentifier).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(p => !new[] { "pluginAssemblyName", "pluginClassName", "currentDirectory" }.Contains(p.Name));
        }

        [Fact]
        public void Sets_Properties_Correctly_On_Empty_CurrentDirectory()
        {
            // Act
            var instance = new FormattableStringTemplateIdentifier("some template", CultureInfo.CurrentCulture, "AssemblyName", "ClassName", null);

            // Assert
            instance.CurrentDirectory.Should().Be(Directory.GetCurrentDirectory());
            instance.FormatProvider.Should().Be(CultureInfo.CurrentCulture);
            instance.PluginAssemblyName.Should().Be("AssemblyName");
            instance.PluginClassName.Should().Be("ClassName");
        }

        [Fact]
        public void Sets_properties_Correctly_On_Non_Empty_CurrentDirectory()
        {
            // Act
            var instance = new FormattableStringTemplateIdentifier("some template", CultureInfo.CurrentCulture, "AssemblyName", "ClassName", "Dir");

            // Assert
            instance.CurrentDirectory.Should().Be("Dir");
            instance.FormatProvider.Should().Be(CultureInfo.CurrentCulture);
            instance.PluginAssemblyName.Should().Be("AssemblyName");
            instance.PluginClassName.Should().Be("ClassName");
        }
    }
}
