namespace TemplateFramework.Core.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_Template()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .BuildServiceProvider();
        var sut = provider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.Template(builder => builder.Append("Hello world!"));
        var fileSystemMock = new Mock<IFileSystem>();
        var generationEnvironment = new MultipleContentBuilder(fileSystemMock.Object, Encoding.UTF8, TestData.BasePath);

        // Act
        sut.Render(new RenderTemplateRequest(template, generationEnvironment));

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Hello world!");
    }
}
