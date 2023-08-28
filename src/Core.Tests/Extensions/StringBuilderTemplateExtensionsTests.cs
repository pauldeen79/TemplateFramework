namespace TemplateFramework.Core.Tests.Extensions;

public class StringBuilderTemplateExtensionsTests
{
    protected Mock<IStringBuilderTemplate> SutMock { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IContentBuilder> ContentBuilderMock { get; } = new();

    protected const string DefaultFilename = "MyFile.txt";
    protected const bool SkipWhenFileExists = true;
    protected StringBuilder StringBuilder { get; } = new();

    public class RenderToMultipleContentBuilder_Without_SkipWhenFileExists_Argument : StringBuilderTemplateExtensionsTests
    {
        [Fact]
        public void Creates_New_Content_When_DefaultFile_Is_Not_Present_Yet()
        {
            // Arrange
            MultipleContentBuilderMock.SetupGet(x => x.Contents).Returns(new[] { ContentBuilderMock.Object });

            SutMock.Setup(x => x.Render(It.IsAny<StringBuilder>())).Callback(() => StringBuilder.Append("Added data"));

            // Act
            SutMock.Object.RenderToMultipleContentBuilder(MultipleContentBuilderMock.Object, DefaultFilename);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.AddContent(DefaultFilename, false, It.IsAny<StringBuilder>()), Times.Once);
        }
    }

    public class RenderToMultipleContentBuilder_With_SkipWhenFileExists_Argument : StringBuilderTemplateExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Act & Assert
            SutMock.Object.Invoking(x => x.RenderToMultipleContentBuilder(builder: null!, DefaultFilename, SkipWhenFileExists))
                          .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Appends_Data_To_Content_Of_DefaultFile_When_Present()
        {
            // Arrange
            MultipleContentBuilderMock.SetupGet(x => x.Contents).Returns(new[] { ContentBuilderMock.Object });
            ContentBuilderMock.SetupGet(x => x.Filename).Returns(DefaultFilename);
            ContentBuilderMock.SetupGet(x => x.Builder).Returns(StringBuilder);
            StringBuilder.AppendLine("Initial data");
            SutMock.Setup(x => x.Render(It.IsAny<StringBuilder>())).Callback(() => StringBuilder.Append("Added data"));

            // Act
            SutMock.Object.RenderToMultipleContentBuilder(MultipleContentBuilderMock.Object, DefaultFilename, SkipWhenFileExists);

            // Assert
            StringBuilder.ToString().Should().Be(@"Initial data
Added data");
        }

        [Fact]
        public void Creates_New_Content_When_DefaultFile_Is_Not_Present_Yet()
        {
            // Arrange
            MultipleContentBuilderMock.SetupGet(x => x.Contents).Returns(new[] { ContentBuilderMock.Object });

            SutMock.Setup(x => x.Render(It.IsAny<StringBuilder>())).Callback(() => StringBuilder.Append("Added data"));

            // Act
            SutMock.Object.RenderToMultipleContentBuilder(MultipleContentBuilderMock.Object, DefaultFilename, SkipWhenFileExists);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.AddContent(DefaultFilename, SkipWhenFileExists, It.IsAny<StringBuilder>()), Times.Once);
        }
    }
}
