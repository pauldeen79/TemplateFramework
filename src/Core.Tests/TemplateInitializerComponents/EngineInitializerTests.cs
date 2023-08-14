namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class EngineInitializerTests
{
    protected EngineInitializer CreateSut() => new();
    
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : EngineInitializerTests
    {
        [Fact]
        public void Sets_Engine_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var template = new TestData.TemplateWithEngine(_ => { });
            var request = new RenderTemplateRequest(template, model, new StringBuilder(), DefaultFilename);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Engine.Should().BeSameAs(TemplateEngineMock.Object);
        }
    }
}
