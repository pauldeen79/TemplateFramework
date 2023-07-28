namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class Constructor : MultipleContentBuilderTests
    {
        [Fact]
        public void Creates_Instance()
        {
            // Act
            var sut = new MultipleContentBuilder();

            // Assert
            sut.Should().NotBeNull();
        }
    }
}
