namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblySettingsTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssemblySettings(TestData.BasePath, assemblyName: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssemblySettings(basePath: null!, TestData.GetAssemblyName()))
                .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }

        [Fact]
        public void Constructs_With_BasePath_And_AssemblyName()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName());

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
        }

        [Fact]
        public void Constructs_With_BasePath_And_AssemblyName_And_CurrentDirectory()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"));

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
        }

        [Fact]
        public void Constructs_With_BasePath_And_AssemblyName_And_DryRun()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), dryRun: true);

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Constructs_With_BasePath_And_AssemblyName_And_CurrentDirectory_And_ClassNameFilter()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"), classNameFilter: new[] { "MyFilter" });

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
            instance.ClassNameFilter.Should().BeEquivalentTo("MyFilter");
        }

        [Fact]
        public void Constructs_With_BasePath_And_AssemblyName_And_CurrentDirectory_And_DryRun()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"), dryRun: true);

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
            instance.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Constructs_With_All_Arguments()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), true, null, new[] { "Filter" });

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.DryRun.Should().BeTrue();
            instance.CurrentDirectory.Should().NotBeNull();
            instance.ClassNameFilter.Should().BeEquivalentTo("Filter");
        }
    }
}
