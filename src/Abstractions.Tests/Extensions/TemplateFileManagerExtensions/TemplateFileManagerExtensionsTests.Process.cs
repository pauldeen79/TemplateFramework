namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateFileManagerExtensionsTests
{
    public class Process : TemplateFileManagerExtensionsTests
    {
        [Fact]
        public void No_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Process();

            // Assert
            sut.Verify(x => x.Process(true, false), Times.Once);
        }

        [Fact]
        public void Only_Split_Argument_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Process(Split);

            // Assert
            sut.Verify(x => x.Process(Split, false), Times.Once);
        }
    }
}
