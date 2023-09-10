namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    public class DeleteLastGeneratedFiles : MultipleContentBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: null!, false))
               .Should().Throw<ArgumentNullException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Empty_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: string.Empty, false))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_WhiteSpace_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: " ", false))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, encoding: null!, "LastGeneratedFiles.txt", true))
               .Should().Throw<ArgumentNullException>().WithParameterName("encoding");
        }

        [Fact]
        public void Appends_Directory_To_BasePath_When_LastGeneratedFilesPath_Contains_PathSeparator()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, Path.Combine("MyDirectory", "LastGeneratedFiles.txt"), false);

            // Assert
            FileSystemMock.Received().FileExists(Path.Combine(TestData.BasePath, "MyDirectory", "LastGeneratedFiles.txt"));
        }

        [Fact]
        public void Does_Not_Append_Anything_To_BasePath_When_LastGeneratedFilesPath_Does_Not_Contain_PathSeparator_And_BasePath_Is_Not_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received().FileExists(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"));
        }

        [Fact]
        public void Does_Not_Append_Anything_To_BasePath_When_LastGeneratedFilesPath_Does_Not_Contain_PathSeparator_And_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received().FileExists("LastGeneratedFiles.txt");
        }

        [Fact]
        public void Deletes_No_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_But_Directory_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(false);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.generated.cs", false);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>());
            FileSystemMock.DidNotReceive().FileDelete(Arg.Any<string>());
        }

        [Fact]
        public void Deletes_No_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_But_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "*.generated.cs", false);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>());
            FileSystemMock.DidNotReceive().FileDelete(Arg.Any<string>());
        }

        [Fact]
        public void Deletes_All_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_Without_Recursion()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);
            FileSystemMock.GetFiles(TestData.BasePath, "*.generated.cs", false).Returns(new[] { Path.Combine(TestData.BasePath, "File1.txt") });

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.generated.cs", false);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>()); // not called because the files are read from Directory.GetFiles
            FileSystemMock.Received().FileDelete(Arg.Any<string>()); // called once because Directory.GetFiles returns one file
        }

        [Fact]
        public void Deletes_All_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_With_Recursion()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);
            FileSystemMock.GetFiles(TestData.BasePath, "*.generated.cs", true).Returns(new[]
            {
                Path.Combine(TestData.BasePath, "File1.txt"),
                Path.Combine(TestData.BasePath, "Subdirectory", "File2.txt")
            });

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.generated.cs", true);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>()); // not called because the files are read from Directory.GetFiles
            FileSystemMock.Received(2).FileDelete(Arg.Any<string>()); // called once because Directory.GetFiles returns two files
        }

        [Fact]
        public void Deletes_No_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_But_File_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists("LastGeneratedFiles.txt").Returns(false);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received().FileExists(Arg.Any<string>()); // once because last generated files is checked
            FileSystemMock.DidNotReceive().FileDelete(Arg.Any<string>());
        }

        [Fact]
        public void Deletes_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_And_File_Exists_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists(Arg.Any<string>())
                          .Returns(x => x.ArgAt<string>(0) == "LastGeneratedFiles.txt" || x.ArgAt<string>(0) == "File1.txt");
            FileSystemMock.ReadAllLines("LastGeneratedFiles.txt", Arg.Any<Encoding>()).Returns(new[]
            {
                "File1.txt",
                "File2.txt"
            });

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received(3).FileExists(Arg.Any<string>()); // once because last generated files is checked, two times because of File1.txt and File2.txt
            FileSystemMock.Received(1).FileDelete(Arg.Any<string>()); // File2.txt does not exist, so only File1.txt is deleted
        }

        [Fact]
        public void Deletes_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_And_File_Exists_Non_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0) == Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt") || x.ArgAt<string>(0) == Path.Combine(TestData.BasePath, "File1.txt"));
            FileSystemMock.ReadAllLines(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"), Arg.Any<Encoding>()).Returns(new[]
            {
                "File1.txt",
                "File2.txt"
            });

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received(3).FileExists(Arg.Any<string>()); // once because last generated files is checked, two times because of File1.txt and File2.txt
            FileSystemMock.Received(1).FileDelete(Arg.Any<string>()); // File2.txt does not exist, so only File1.txt is deleted
        }
    }
}
