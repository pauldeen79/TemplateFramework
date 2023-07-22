namespace TemplateFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class SaveLastGeneratedFiles : TemplateFileManagerTests
    {
        [Theory,
            InlineData(null),
            InlineData(""),
            InlineData(" ")]
        public void Throws_On_Invalid_LastGeneratedFilesPath(string lastGeneratedFilesPath)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.SaveLastGeneratedFiles(lastGeneratedFilesPath))
               .Should().Throw<ArgumentException>().WithParameterName(nameof(lastGeneratedFilesPath));
        }

        [Fact]
        public void Saves_Files_On_Valid_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.SaveLastGeneratedFiles("LastGeneratedFiles.txt");

            // Assert
            MultipleContentBuilderMock.Verify(x => x.SaveLastGeneratedFiles("LastGeneratedFiles.txt"), Times.Once);
        }
    }
}
