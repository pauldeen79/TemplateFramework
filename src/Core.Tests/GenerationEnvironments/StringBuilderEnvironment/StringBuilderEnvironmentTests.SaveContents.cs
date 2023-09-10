namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class StringBuilderEnvironmentTests
{
    public class SaveContents : StringBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Provider()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveContents(provider: null!, TestData.BasePath, "Filename.txt"))
               .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Empty_DefaultFilename()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveContents(CodeGenerationProviderMock, TestData.BasePath, defaultFilename: null!))
               .Should().Throw<ArgumentException>().WithParameterName("defaultFilename");
        }

        [Fact]
        public void Throws_On_Null_DefaultFilename()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveContents(CodeGenerationProviderMock, TestData.BasePath, defaultFilename: string.Empty))
               .Should().Throw<ArgumentException>().WithParameterName("defaultFilename");
        }

        [Fact]
        public void Writes_Contents_To_FileSystem_Without_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            sut.SaveContents(CodeGenerationProviderMock, string.Empty, "Filename.txt");

            // Arrange
            FileSystemMock.Received().WriteAllText("Filename.txt", "Contents", Encoding.UTF32);
        }

        [Fact]
        public void Writes_Contents_To_FileSystem_With_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, "Filename.txt");

            // Arrange
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "Filename.txt"), "Contents", Encoding.UTF32);
        }
    }
}
