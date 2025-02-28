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
            instance.BasePath.ShouldBe(TestData.BasePath);
            instance.DefaultFilename.ShouldBe("DefaultFilename.txt");
            instance.AssemblyName.ShouldBe(TestData.GetAssemblyName());
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_CurrentDirectory()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"));

            // Assert
            instance.BasePath.ShouldBe(TestData.BasePath);
            instance.DefaultFilename.ShouldBe("DefaultFilename.txt");
            instance.AssemblyName.ShouldBe(TestData.GetAssemblyName());
            instance.CurrentDirectory.ShouldBe(Path.Combine(TestData.BasePath, "SomeDirectory"));
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_DryRun()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), dryRun: true);

            // Assert
            instance.BasePath.ShouldBe(TestData.BasePath);
            instance.DefaultFilename.ShouldBe("DefaultFilename.txt");
            instance.AssemblyName.ShouldBe(TestData.GetAssemblyName());
            instance.DryRun.ShouldBeTrue();
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_CurrentDirectory_And_ClassNameFilter()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"), classNameFilter: ["MyFilter"]);

            // Assert
            instance.BasePath.ShouldBe(TestData.BasePath);
            instance.DefaultFilename.ShouldBe("DefaultFilename.txt");
            instance.AssemblyName.ShouldBe(TestData.GetAssemblyName());
            instance.CurrentDirectory.ShouldBe(Path.Combine(TestData.BasePath, "SomeDirectory"));
            instance.ClassNameFilter.ToArray().ShouldBeEquivalentTo(new[] { "MyFilter" });
        }

        [Fact]
        public void Constructs_With_BasePath_And_DefaultFilename_And_AssemblyName_And_CurrentDirectory_And_DryRun()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), Path.Combine(TestData.BasePath, "SomeDirectory"), dryRun: true);

            // Assert
            instance.BasePath.ShouldBe(TestData.BasePath);
            instance.DefaultFilename.ShouldBe("DefaultFilename.txt");
            instance.AssemblyName.ShouldBe(TestData.GetAssemblyName());
            instance.CurrentDirectory.ShouldBe(Path.Combine(TestData.BasePath, "SomeDirectory"));
            instance.DryRun.ShouldBeTrue();
        }

        [Fact]
        public void Constructs_With_All_Arguments()
        {
            // Act
            var instance = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), true, null, ["Filter"]);

            // Assert
            instance.BasePath.ShouldBe(TestData.BasePath);
            instance.DefaultFilename.ShouldBe("DefaultFilename.txt");
            instance.AssemblyName.ShouldBe(TestData.GetAssemblyName());
            instance.DryRun.ShouldBeTrue();
            instance.CurrentDirectory.ShouldNotBeNull();
            instance.ClassNameFilter.ToArray().ShouldBeEquivalentTo(new[] { "Filter" });
        }
    }
}
