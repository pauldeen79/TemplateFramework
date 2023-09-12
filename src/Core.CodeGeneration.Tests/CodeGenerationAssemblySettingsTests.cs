namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationAssemblySettingsTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(CodeGenerationAssemblySettings).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                parameterPredicate: p => !new[] { "currentDirectory", "classNameFilter" }.Contains(p.Name), // this parameter is optional
                parameterReplaceDelegate: p => p.Name switch
                {
                    "basePath" => TestData.BasePath,
                    "defaultFilename" => "DefaultFilename.txt",
                    "assemblyName" => TestData.GetAssemblyName(),
                    _ => null
                });
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName());

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.DefaultFilename.Should().Be("DefaultFilename.txt");
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_CurrentDirectory()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"));

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.DefaultFilename.Should().Be("DefaultFilename.txt");
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_DryRun()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), dryRun: true);

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.DefaultFilename.Should().Be("DefaultFilename.txt");
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_CurrentDirectory_And_ClassNameFilter()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"), classNameFilter: new[] { "MyFilter" });

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.DefaultFilename.Should().Be("DefaultFilename.txt");
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
            instance.ClassNameFilter.Should().BeEquivalentTo("MyFilter");
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_CurrentDirectory_And_DryRun()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"), dryRun: true);

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.DefaultFilename.Should().Be("DefaultFilename.txt");
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.CurrentDirectory.Should().Be(Path.Combine(TestData.BasePath, "SomeDirectory"));
            instance.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Constructs_With_All_Arguments()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), true, null, new[] { "Filter" });

            // Assert
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.DefaultFilename.Should().Be("DefaultFilename.txt");
            instance.AssemblyName.Should().Be(TestData.GetAssemblyName());
            instance.DryRun.Should().BeTrue();
            instance.CurrentDirectory.Should().NotBeNull();
            instance.ClassNameFilter.Should().BeEquivalentTo("Filter");
        }
    }
}
