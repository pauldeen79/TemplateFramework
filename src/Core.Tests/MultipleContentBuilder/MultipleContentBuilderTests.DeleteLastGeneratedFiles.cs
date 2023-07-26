namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class DeleteLastGeneratedFiles : MultipleContentBuilderTests
    {
        [Fact]
        public void Throws_On_Null_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles(lastGeneratedFilesPath: null!, false, Encoding.Latin1))
               .Should().Throw<ArgumentNullException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Empty_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles(lastGeneratedFilesPath: string.Empty, false, Encoding.Latin1))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_WhiteSpace_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles(lastGeneratedFilesPath: " ", false, Encoding.Latin1))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", true, encoding: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("encoding");
        }

        [Fact]
        public void Appends_Directory_To_BasePath_When_LastGeneratedFilesPath_Contains_PathSeparator()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act
            sut.DeleteLastGeneratedFiles(Path.Combine("MyDirectory", "LastGeneratedFiles.txt"), false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(Path.Combine(TestData.BasePath, "MyDirectory", "LastGeneratedFiles.txt")), Times.Once);
        }

        [Fact]
        public void Does_Not_Append_Anything_To_BasePath_When_LastGeneratedFilesPath_Does_Not_Contain_PathSeparator_And_BasePath_Is_Not_Empty()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act
            sut.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt")), Times.Once);
        }

        [Fact]
        public void Does_Not_Append_Anything_To_BasePath_When_LastGeneratedFilesPath_Does_Not_Contain_PathSeparator_And_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut(string.Empty);

            // Act
            sut.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists("LastGeneratedFiles.txt"), Times.Once);
        }

        [Fact]
        public void Deletes_No_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_But_Directory_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(false);

            // Act
            sut.DeleteLastGeneratedFiles("*.generated.cs", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(It.IsAny<string>()), Times.Never);
            FileSystemMock.Verify(x => x.FileDelete(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Deletes_No_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_But_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut(string.Empty);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(true);

            // Act
            sut.DeleteLastGeneratedFiles("*.generated.cs", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(It.IsAny<string>()), Times.Never);
            FileSystemMock.Verify(x => x.FileDelete(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Deletes_All_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_Without_Recursion()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(true);
            FileSystemMock.Setup(x => x.GetFiles(TestData.BasePath, "*.generated.cs", false)).Returns(new[] { Path.Combine(TestData.BasePath, "File1.txt") });

            // Act
            sut.DeleteLastGeneratedFiles("*.generated.cs", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(It.IsAny<string>()), Times.Never); // not called because the files are read from Directory.GetFiles
            FileSystemMock.Verify(x => x.FileDelete(It.IsAny<string>()), Times.Once); // called once because Directory.GetFiles returns one file
        }

        [Fact]
        public void Deletes_All_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_With_Recursion()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(true);
            FileSystemMock.Setup(x => x.GetFiles(TestData.BasePath, "*.generated.cs", true)).Returns(new[]
            {
                Path.Combine(TestData.BasePath, "File1.txt"),
                Path.Combine(TestData.BasePath, "Subdirectory", "File2.txt")
            });

            // Act
            sut.DeleteLastGeneratedFiles("*.generated.cs", true, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(It.IsAny<string>()), Times.Never); // not called because the files are read from Directory.GetFiles
            FileSystemMock.Verify(x => x.FileDelete(It.IsAny<string>()), Times.Exactly(2)); // called once because Directory.GetFiles returns two files
        }

        [Fact]
        public void Deletes_No_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_But_File_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut(string.Empty);
            FileSystemMock.Setup(x => x.FileExists("LastGeneratedFiles.txt")).Returns(false);

            // Act
            sut.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(It.IsAny<string>()), Times.Once); // once because last generated files is checked
            FileSystemMock.Verify(x => x.FileDelete(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Deletes_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_And_File_Exists_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut(string.Empty);
            FileSystemMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns<string>(path => path == "LastGeneratedFiles.txt" || path == "File1.txt");
            FileSystemMock.Setup(x => x.ReadAllLines("LastGeneratedFiles.txt", It.IsAny<Encoding>())).Returns(new[]
            {
                "File1.txt",
                "File2.txt"
            });

            // Act
            sut.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(It.IsAny<string>()), Times.Exactly(3)); // once because last generated files is checked, two times because of File1.txt and File2.txt
            FileSystemMock.Verify(x => x.FileDelete(It.IsAny<string>()), Times.Exactly(1)); // File2.txt does not exist, so only File1.txt is deleted
        }

        [Fact]
        public void Deletes_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_And_File_Exists_Non_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns<string>(path => path == Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt") || path == Path.Combine(TestData.BasePath, "File1.txt"));
            FileSystemMock.Setup(x => x.ReadAllLines(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"), It.IsAny<Encoding>())).Returns(new[]
            {
                "File1.txt",
                "File2.txt"
            });

            // Act
            sut.DeleteLastGeneratedFiles("LastGeneratedFiles.txt", false, Encoding.Latin1);

            // Assert
            FileSystemMock.Verify(x => x.FileExists(It.IsAny<string>()), Times.Exactly(3)); // once because last generated files is checked, two times because of File1.txt and File2.txt
            FileSystemMock.Verify(x => x.FileDelete(It.IsAny<string>()), Times.Exactly(1)); // File2.txt does not exist, so only File1.txt is deleted
        }
    }
}
