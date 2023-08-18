namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ProviderInitializerTests
{
    protected ProviderInitializer CreateSut() => new(TemplateProviderMock.Object);
    
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : ProviderInitializerTests
    {
        [Fact]
        public void Sets_Engine_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var template = new TestData.TemplateWithProvider(_ => { });
            var request = new RenderTemplateRequest(template, model, new StringBuilder(), DefaultFilename);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Provider.Should().BeSameAs(TemplateProviderMock.Object);
        }
    }
}
