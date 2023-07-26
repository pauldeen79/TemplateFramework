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
    }
}
