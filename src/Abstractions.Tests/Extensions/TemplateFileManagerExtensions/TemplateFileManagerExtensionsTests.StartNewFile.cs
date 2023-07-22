namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateFileManagerExtensionsTests
{
    public class StartNewFile : TemplateFileManagerExtensionsTests
    {
        [Fact]
        public void No_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            _ = sut.Object.StartNewFile();

            // Assert
            sut.Verify(x => x.StartNewFile(string.Empty, false), Times.Once);
        }

        [Fact]
        public void Only_Filename_Argument_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            _ = sut.Object.StartNewFile(Filename);

            // Assert
            sut.Verify(x => x.StartNewFile(Filename, false), Times.Once);
        }
    }
}
