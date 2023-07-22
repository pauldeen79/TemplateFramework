namespace TemplateFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class Constructor : TemplateFileManagerTests
    {
        [Fact]
        public void Throws_On_Null_StringBuilder()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateFileManager(stringBuilder: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("stringBuilder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateFileManager(multipleContentBuilder: null!, new StringBuilder()))
                .Should().Throw<ArgumentNullException>().WithParameterName("multipleContentBuilder");
        }

        [Fact]
        public void Creates_Instance_With_StringBuilder()
        {
            // Arrange
            var stringBuilder = new StringBuilder();

            // Act
            var sut = new TemplateFileManager(stringBuilder: stringBuilder);
            sut.ResetToDefaultOutput(); // needed to check the sent StringBuilder

            // Assert
            sut.MultipleContentBuilder.Should().NotBeNull();
            sut.MultipleContentBuilder.BasePath.Should().BeEmpty();
            sut.GenerationEnvironment.Should().BeSameAs(stringBuilder);
        }

        [Fact]
        public void Creates_Instance_With_No_Arguments()
        {
            // Act
            var sut = new TemplateFileManager();

            // Assert
            sut.MultipleContentBuilder.Should().NotBeNull();
            sut.MultipleContentBuilder.BasePath.Should().BeEmpty();
            sut.GenerationEnvironment.Should().NotBeNull();
        }

        [Fact]
        public void Creates_Instance_With_MultipleContentBuilder()
        {
            // Arrange
            var multipleContentBuilder = MultipleContentBuilderMock.Object;

            // Act
            var sut = new TemplateFileManager(multipleContentBuilder: multipleContentBuilder);

            // Assert
            sut.MultipleContentBuilder.Should().BeSameAs(multipleContentBuilder);
            sut.GenerationEnvironment.Should().NotBeNull();
        }
    }
}
