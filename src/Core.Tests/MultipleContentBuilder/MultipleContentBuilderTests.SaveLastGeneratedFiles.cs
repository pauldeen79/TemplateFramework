namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class SaveLastGeneratedFiles : MultipleContentBuilderTests
    {
        [Fact]
        public void Throws_On_Null_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(lastGeneratedFilesPath: null!, Encoding.Latin1))
               .Should().Throw<ArgumentNullException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Empty_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(lastGeneratedFilesPath: string.Empty, Encoding.Latin1))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_WhiteSpace_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(lastGeneratedFilesPath: " ", Encoding.Latin1))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles("LastGeneratedFiles.txt", encoding: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("encoding");
        }

        [Fact]
        public void Creates_Directory_When_It_Does_Not_Exist_Yet()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(false);

            // Act
            sut.SaveLastGeneratedFiles("LastGeneratedFiles.txt", Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.CreateDirectory(TestData.BasePath), Times.Once);
        }

        [Fact]
        public void Creates_File_When_Filename_Does_Not_Contain_Asterisk_Using_NonEmpty_BasePath()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles("LastGeneratedFiles.txt", Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.WriteAllLines(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"), It.IsAny<IEnumerable<string>>(), Encoding.Latin1), Times.Once);
        }

        [Fact]
        public void Creates_File_When_Filename_Does_Not_Contain_Asterisk_Using_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut(string.Empty);
            FileSystemMock.Setup(x => x.DirectoryExists("MyDirectory")).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles("LastGeneratedFiles.txt", Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.WriteAllLines("LastGeneratedFiles.txt", It.IsAny<IEnumerable<string>>(), Encoding.Latin1), Times.Once);
        }

        [Fact]
        public void Does_Not_Create_File_When_Filename_Contains_Asterisk()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles("*.template.generated.cs", Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.WriteAllLines(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), Encoding.Latin1), Times.Never);
        }
    }
}
