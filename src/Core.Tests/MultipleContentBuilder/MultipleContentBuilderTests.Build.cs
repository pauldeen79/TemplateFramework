namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class Build : MultipleContentBuilderTests
    {
        [Fact]
        public void Generates_MultipleContent_Instance()
        {
            // Arrange
            var sut = CreateSut(skipWhenFileExists: true);

            // Act
            var instance = sut.Build();

            // Assert
            instance.Should().NotBeNull();
            instance.Contents.Should().HaveCount(2);
            instance.Contents.Select(x => x.SkipWhenFileExists).Should().AllBeEquivalentTo(true);
        }
    }
}
