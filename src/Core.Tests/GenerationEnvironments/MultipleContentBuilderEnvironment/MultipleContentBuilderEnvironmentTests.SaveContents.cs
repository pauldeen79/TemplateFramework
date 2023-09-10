namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    public class SaveContents : MultipleContentBuilderEnvironmentTests
    {
        public SaveContents()
        {
            CodeGenerationProviderMock.LastGeneratedFilesFilename.Returns("LastGeneratedFiles.txt");
            CodeGenerationProviderMock.Encoding.Returns(Encoding.Latin1);
            CodeGenerationProviderMock.Path.Returns("Subdirectory");
            ContentMock.Filename.Returns(Path.Combine("Subdirectory", "Filename.txt"));
            ContentMock.Contents.Returns("Content");
            MultipleContentMock.Contents.Returns(new[] { ContentMock }.ToList().AsReadOnly());
            MultipleContentBuilderMock.Build().Returns(MultipleContentMock);
        }

        [Fact]
        public void Deletes_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Not_Empty()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists(Path.Combine(TestData.BasePath, "Subdirectory", "LastGeneratedFiles.txt")).Returns(true);

            // Act
            sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, string.Empty);

            // Assert
            FileSystemMock.Received().ReadAllLines(Path.Combine(TestData.BasePath, "Subdirectory", "LastGeneratedFiles.txt"), Encoding.Latin1);
        }

        [Fact]
        public void Does_Not_Delete_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.LastGeneratedFilesFilename.Returns(default(string)!);

            // Act
            sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, string.Empty);

            // Assert
            FileSystemMock.DidNotReceive().ReadAllLines(Arg.Any<string>(), Arg.Any<Encoding>());
        }

        [Fact]
        public void Does_Not_Delete_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.LastGeneratedFilesFilename.Returns(string.Empty);

            // Act
            sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, string.Empty);

            // Assert
            FileSystemMock.DidNotReceive().ReadAllLines(Arg.Any<string>(), Arg.Any<Encoding>());
        }

        [Fact]
        public void Saves_All_Contents()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, string.Empty);

            // Assert
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "Subdirectory", "Filename.txt"), "Content", Encoding.Latin1);
        }
    }
}
