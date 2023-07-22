namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateFileManagerExtensionsTests
{
    public class DeleteLastGeneratedFiles : TemplateFileManagerExtensionsTests
    {
        [Fact]
        public void Only_LastGeneratedFilesPath_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.DeleteLastGeneratedFiles(LastGeneratedFilesPath);

            // Assert
            sut.Verify(x => x.DeleteLastGeneratedFiles(LastGeneratedFilesPath, true), Times.Once);
        }
    }
}
