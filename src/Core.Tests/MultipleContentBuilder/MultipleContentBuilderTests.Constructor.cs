namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class Constructor : MultipleContentBuilderTests
    {
        [Fact]
        public void Creates_Instance_With_Empty_BasePath()
        {
            // Act
            var sut = new MultipleContentBuilder();

            // Assert
            sut.BasePath.Should().BeEmpty();
        }

        [Fact]
        public void Creates_Instance_With_Filled_BasePath()
        {
            // Act
            var sut = new MultipleContentBuilder(TestData.BasePath);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentBuilder(basePath: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }
    }
}
