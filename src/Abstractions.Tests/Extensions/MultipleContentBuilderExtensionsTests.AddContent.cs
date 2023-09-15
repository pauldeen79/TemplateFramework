namespace TemplateFramework.Abstractions.Tests.Extensions;

public class MultipleContentBuilderExtensionsTests : TestBase
{
    public class AddContent : MultipleContentBuilderExtensionsTests
    {
        [Fact]
        public void Without_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = Fixture.Freeze<IMultipleContentBuilder>();

            // Act
            sut.AddContent();

            // Assert
            sut.Received().AddContent(string.Empty, false, null);
        }

        [Fact]
        public void With_Filename_Argument_Works_Correctly()
        {
            // Arrange
            var sut = Fixture.Freeze<IMultipleContentBuilder>();

            // Act
            sut.AddContent("MyFilename.txt");

            // Assert
            sut.Received().AddContent("MyFilename.txt", false, null);
        }

        [Fact]
        public void With_Filename_And_SkipWhenFileExists_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = Fixture.Freeze<IMultipleContentBuilder>();

            // Act
            sut.AddContent("MyFilename.txt", true);

            // Assert
            sut.Received().AddContent("MyFilename.txt", true, null);
        }
    }
}
