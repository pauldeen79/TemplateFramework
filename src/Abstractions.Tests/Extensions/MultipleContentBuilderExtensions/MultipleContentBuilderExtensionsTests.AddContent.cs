namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class MultipleContentBuilderExtensionsTests
{
    public class AddContent : MultipleContentBuilderExtensionsTests
    {
        [Fact]
        public void Without_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.AddContent();

            // Assert
            sut.Verify(x => x.AddContent(string.Empty, false, null), Times.Once);
        }

        [Fact]
        public void With_Filename_Argument_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.AddContent("MyFilename.txt");

            // Assert
            sut.Verify(x => x.AddContent("MyFilename.txt", false, null), Times.Once);
        }

        [Fact]
        public void With_Filename_And_SkipWhenFileExists_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.AddContent("MyFilename.txt", true);

            // Assert
            sut.Verify(x => x.AddContent("MyFilename.txt", true, null), Times.Once);
        }
    }
}
