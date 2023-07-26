namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class StringBuilderEnvironmentTests
{
    public class Process : StringBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Provider()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process(provider: null!, TestData.BasePath))
               .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Writes_Contents_To_FileSystem_Without_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("Filename.txt");
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            sut.Process(CodeGenerationProviderMock.Object, string.Empty);

            // Arrange
            FileSystemMock.Verify(x => x.WriteAllText("Filename.txt", "Contents", Encoding.UTF32), Times.Once);
        }

        [Fact]
        public void Writes_Contents_To_FileSystem_With_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("Filename.txt");
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            sut.Process(CodeGenerationProviderMock.Object, TestData.BasePath);

            // Arrange
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "Filename.txt"), "Contents", Encoding.UTF32), Times.Once);
        }
    }
}
