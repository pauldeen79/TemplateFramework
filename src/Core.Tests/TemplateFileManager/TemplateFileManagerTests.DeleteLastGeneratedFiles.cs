namespace TemplateFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class DeleteLastGeneratedFiles : TemplateFileManagerTests
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
            sut.Invoking(x => x.DeleteLastGeneratedFiles(lastGeneratedFilesPath))
               .Should().Throw<ArgumentException>().WithParameterName(nameof(lastGeneratedFilesPath));
        }

        [Theory,
            InlineData(true),
            InlineData(false)]
        public void Deletes_Files_On_Valid_LastGeneratedFilesPath(bool recurse)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", recurse);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", recurse), Times.Once);
        }
    }
}
