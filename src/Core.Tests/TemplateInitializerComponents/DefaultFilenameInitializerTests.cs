namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class DefaultFilenameInitializerTests
{
    protected DefaultFilenameInitializer CreateSut() => new();
    
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Initialize : DefaultFilenameInitializerTests
    {
        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(request: null!, TemplateEngineMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_On_Null_Engine()
        {
            // Arrange
            var sut = CreateSut();
            var template = this;
            var request = new RenderTemplateRequest(template, null, new StringBuilder(), DefaultFilename);

            // Act & Assert
            sut.Invoking(x => x.Initialize(request, engine: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("engine");
        }

        [Fact]
        public void Sets_DefaultFilename_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithDefaultFilename(_ => { });
            var request = new RenderTemplateRequest(template, null, new StringBuilder(), DefaultFilename);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.DefaultFilename.Should().Be(DefaultFilename);
        }
    }
}
