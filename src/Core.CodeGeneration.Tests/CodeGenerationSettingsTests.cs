namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationSettingsTests
{
    protected const bool DryRun = true;

    public class Constructor : CodeGenerationSettingsTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(CodeGenerationSettings).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                parameterPredicate: p => new[] { "basePath", "defaultFilename" }.Contains(p.Name));
        }

        [Fact]
        public void Creates_Instance_With_Basepath()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
        }

        [Fact]
        public void Creates_Instance_With_Basepath_And_DefaultFilename()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath, "DefaultFilename.txt");

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
            sut.DefaultFilename.Should().Be("DefaultFilename.txt");
        }

        [Fact]
        public void Creates_Instance_With_BasePath_And_DefaultFilename_And_DryRun()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath, "DefaultFilename.txt", DryRun);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
            sut.DefaultFilename.Should().Be("DefaultFilename.txt");
            sut.DryRun.Should().Be(DryRun);
        }
    }
}
