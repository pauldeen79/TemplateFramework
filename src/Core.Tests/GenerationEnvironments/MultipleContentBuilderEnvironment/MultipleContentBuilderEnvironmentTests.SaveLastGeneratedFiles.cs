namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    public class SaveLastGeneratedFiles : MultipleContentBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: null!, CreateContents() ))
               .Should().Throw<ArgumentNullException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Empty_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: string.Empty, CreateContents()))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_WhiteSpace_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: " ", CreateContents()))
               .Should().Throw<ArgumentException>().WithParameterName("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, encoding: null!, "LastGeneratedFiles.txt", CreateContents()))
               .Should().Throw<ArgumentNullException>().WithParameterName("encoding");
        }

        [Fact]
        public void Throws_On_Null_Contents()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", contents: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("contents");
        }

        [Fact]
        public void Creates_Directory_When_It_Does_Not_Exist_Yet()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(false);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", CreateContents());

            // Assert
            FileSystemMock.Received().CreateDirectory(TestData.BasePath);
        }

        [Fact]
        public void Creates_File_When_Filename_Does_Not_Contain_Asterisk_Using_NonEmpty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", CreateContents());

            // Assert
            FileSystemMock.Received().WriteAllLines(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"), Arg.Any<IEnumerable<string>>(), Encoding.Latin1);
        }

        [Fact]
        public void Creates_File_When_Filename_Does_Not_Contain_Asterisk_Using_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists("MyDirectory").Returns(true);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", CreateContents());

            // Assert
            FileSystemMock.Received().WriteAllLines("LastGeneratedFiles.txt", Arg.Any<IEnumerable<string>>(), Encoding.Latin1);
        }

        [Fact]
        public void Does_Not_Create_File_When_Filename_Contains_Asterisk()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.template.generated.cs", CreateContents());

            // Assert
            FileSystemMock.DidNotReceive().WriteAllLines(Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), Encoding.Latin1);
        }
    }
}
