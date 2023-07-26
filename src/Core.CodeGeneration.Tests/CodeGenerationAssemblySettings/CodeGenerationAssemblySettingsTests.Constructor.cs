namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblySettingsTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssemblySettings(assemblyName: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Constructs_With_AssemblyName()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.GetAssemblyName());

            // Assert
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
        }

        [Fact]
        public void Constructs_With_AssemblyName_And_CurrentDirectory()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"));

            // Assert
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
        }

        [Fact]
        public void Constructs_With_AssemblyName_And_DryRun()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.GetAssemblyName(), dryRun: true);

            // Assert
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Constructs_With_AssemblyName_And_CurrentDirectory_And_ClassNameFilter()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"), classNameFilter: new[] { "MyFilter" });

            // Assert
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
            instance.ClassNameFilter.Should().BeEquivalentTo("MyFilter");
        }

        [Fact]
        public void Constructs_With_All_Arguments()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.GetAssemblyName(), true, null, new[] { "Filter" });

            // Assert
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.DryRun.Should().BeTrue();
            instance.CurrentDirectory.Should().NotBeNull();
            instance.ClassNameFilter.Should().BeEquivalentTo("Filter");
        }
    }
}
