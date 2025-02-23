namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests.TemplateIdentifiers;

public class FormattableStringTemplateIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(FormattableStringTemplateIdentifier).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(p => !new[] { "pluginAssemblyName", "pluginClassName", "currentDirectory" }.Contains(p.Name));
        }

        [Fact]
        public void Sets_Properties_Correctly_On_Empty_CurrentDirectory()
        {
            // Act
            var instance = new FormattableStringTemplateIdentifier("some template", CultureInfo.CurrentCulture, "AssemblyName", "ClassName", null);

            // Assert
            instance.CurrentDirectory.ShouldBe(Directory.GetCurrentDirectory());
            instance.FormatProvider.ShouldBe(CultureInfo.CurrentCulture);
            instance.PluginAssemblyName.ShouldBe("AssemblyName");
            instance.PluginClassName.ShouldBe("ClassName");
        }

        [Fact]
        public void Sets_properties_Correctly_On_Non_Empty_CurrentDirectory()
        {
            // Act
            var instance = new FormattableStringTemplateIdentifier("some template", CultureInfo.CurrentCulture, "AssemblyName", "ClassName", "Dir");

            // Assert
            instance.CurrentDirectory.ShouldBe("Dir");
            instance.FormatProvider.ShouldBe(CultureInfo.CurrentCulture);
            instance.PluginAssemblyName.ShouldBe("AssemblyName");
            instance.PluginClassName.ShouldBe("ClassName");
        }
    }
}
