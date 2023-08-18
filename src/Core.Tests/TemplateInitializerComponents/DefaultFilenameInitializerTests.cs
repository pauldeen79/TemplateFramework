namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class DefaultFilenameInitializerTests
{
    protected DefaultFilenameInitializer CreateSut() => new();
    
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : DefaultFilenameInitializerTests
    {
        [Fact]
        public void Sets_DefaultFilename_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var template = new TestData.TemplateWithDefaultFilename(_ => { });
            var request = new RenderTemplateRequest(template, model, new StringBuilder(), DefaultFilename);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.DefaultFilename.Should().Be(DefaultFilename);
        }
    }
}
