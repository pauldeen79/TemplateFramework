namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class DefaultFilenameInitializerTests
{
    protected DefaultFilenameInitializerComponent CreateSut() => new();
    
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : DefaultFilenameInitializerTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Sets_DefaultFilename_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithDefaultFilename(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), null, new StringBuilder(), DefaultFilename);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, TemplateProviderMock.Object, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.DefaultFilename.Should().Be(DefaultFilename);
        }
    }
}
