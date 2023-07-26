namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    public class Process : MultipleContentBuilderEnvironmentTests
    {
        public Process()
        {
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns("LastGeneratedFiles.txt");
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns("Subdirectory");
            ContentMock.SetupGet(x => x.Filename).Returns(Path.Combine("Subdirectory", "Filename.txt"));
            ContentMock.SetupGet(x => x.Contents).Returns("Content");
            MultipleContentMock.SetupGet(x => x.Contents).Returns(new[] { ContentMock.Object }.ToList().AsReadOnly());
            MultipleContentBuilderMock.Setup(x => x.Build()).Returns(MultipleContentMock.Object);
        }

        [Fact]
        public void Deletes_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Not_Empty()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.Setup(x => x.FileExists(Path.Combine(TestData.BasePath, "Subdirectory", "LastGeneratedFiles.txt"))).Returns(true);

            // Act
            sut.Process(CodeGenerationProviderMock.Object, TestData.BasePath);

            // Assert
            FileSystemMock.Verify(x => x.ReadAllLines(Path.Combine(TestData.BasePath, "Subdirectory", "LastGeneratedFiles.txt"), Encoding.Latin1), Times.Once);
        }

        [Fact]
        public void Does_Not_Delete_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns(default(string)!);

            // Act
            sut.Process(CodeGenerationProviderMock.Object, TestData.BasePath);

            // Assert
            FileSystemMock.Verify(x => x.ReadAllLines(It.IsAny<string>(), It.IsAny<Encoding>()), Times.Never);
        }

        [Fact]
        public void Does_Not_Delete_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns(string.Empty);

            // Act
            sut.Process(CodeGenerationProviderMock.Object, TestData.BasePath);

            // Assert
            FileSystemMock.Verify(x => x.ReadAllLines(It.IsAny<string>(), It.IsAny<Encoding>()), Times.Never);
        }

        [Fact]
        public void Saves_All_Contents()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Process(CodeGenerationProviderMock.Object, TestData.BasePath);

            // Assert
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "Subdirectory", "Filename.txt"), "Content", Encoding.Latin1), Times.Once);
        }
    }
}
