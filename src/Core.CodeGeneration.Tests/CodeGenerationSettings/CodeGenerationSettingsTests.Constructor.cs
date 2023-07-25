namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationSettingsTests
{
    public class Constructor : CodeGenerationSettingsTests
    {
        [Fact]
        public void Creates_Instance_With_DryRun()
        {
            // Act
            var sut = new CodeGenerationSettings(DryRun);

            // Assert
            sut.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Creates_Instance_With_SkipWhenFileExists_And_DryRun()
        {
            // Act
            var sut = new CodeGenerationSettings(SkipWhenFileExists, DryRun);

            // Assert
            sut.SkipWhenFileExists.Should().BeTrue();
            sut.DryRun.Should().BeTrue();
        }
    }
}
