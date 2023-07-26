namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationSettingsTests
{
    public class Constructor : CodeGenerationSettingsTests
    {
        [Fact]
        public void Creates_Instance_With_Basepath()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
        }

        [Fact]
        public void Creates_Instance_With_BasePath_And_DryRun()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath, DryRun);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
            sut.DryRun.Should().BeTrue();
        }
    }
}
