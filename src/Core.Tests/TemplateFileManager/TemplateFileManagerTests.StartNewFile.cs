namespace TemplateFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class StartNewFile : TemplateFileManagerTests
    {
        [Fact]
        public void Throws_On_Filename_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.StartNewFile(filename: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("filename");
        }

        [Fact]
        public void Adds_New_Content_To_MultipleContentBuilder()
        {
            // Arrange
            var sut = CreateSut();
            MultipleContentBuilderMock.Setup(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>())).Returns<string, bool, StringBuilder>(CreateContentBuilder);

            // Act
            sut.StartNewFile("MyFile.txt");

            // Assert
            MultipleContentBuilderMock.Verify(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>()), Times.Once);
        }

        [Fact]
        public void Sets_GenerationEnvironment_To_Builder_Of_Added_ContentBuilder()
        {
            // Arrange
            var sut = CreateSut();
            IContentBuilder? createdContentBuilder = null;
            MultipleContentBuilderMock.Setup(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>())).Returns<string, bool, StringBuilder>((fileName, skipFileWhenExists, builder) => { createdContentBuilder = CreateContentBuilder(fileName, skipFileWhenExists, builder); return createdContentBuilder; });

            // Act
            sut.StartNewFile("MyFile.txt");

            // Assert
            sut.GenerationEnvironment.Should().BeSameAs(createdContentBuilder?.Builder);
        }

        [Fact]
        public void Returns_Builder_Of_Added_ContentBuilder()
        {
            // Arrange
            var sut = CreateSut();
            IContentBuilder? createdContentBuilder = null;
            MultipleContentBuilderMock.Setup(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>())).Returns<string, bool, StringBuilder>((fileName, skipFileWhenExists, builder) => { createdContentBuilder = CreateContentBuilder(fileName, skipFileWhenExists, builder); return createdContentBuilder; });

            // Act
            var result = sut.StartNewFile("MyFile.txt");

            // Assert
            result.Should().BeSameAs(createdContentBuilder?.Builder);
        }
    }
}
