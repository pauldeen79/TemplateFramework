namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class Properties : MultipleContentBuilderTests
    {
        [Fact]
        public void Can_Change_BasePath()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.BasePath = "something else";

            // Assert
            sut.BasePath.Should().Be("something else");
        }
    }
}
