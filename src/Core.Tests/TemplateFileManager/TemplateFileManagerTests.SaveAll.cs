namespace TemplateFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class SaveAll : TemplateFileManagerTests
    {
        [Fact]
        public void Saves_All_Content()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.SaveAll();

            // Assert
            MultipleContentBuilderMock.Verify(x => x.SaveAll(), Times.Once);
        }
    }
}
