namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class Build : MultipleContentBuilderTests
    {
        [Fact]
        public void Generates_MultipleContent_Instance()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath, skipWhenFileExists: true, Encoding.Latin1);

            // Act
            var instance = sut.Build();

            // Assert
            instance.Should().NotBeNull();
            instance.BasePath.Should().Be(TestData.BasePath);
            instance.Encoding.Should().Be(Encoding.Latin1);
            instance.Contents.Should().HaveCount(2);
            instance.Contents.Select(x => x.SkipWhenFileExists).Should().AllBeEquivalentTo(true);
        }
    }
}
