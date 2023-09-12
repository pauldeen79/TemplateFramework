namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class DefaultFilenameInitializerTests
{
    public class Initialize : DefaultFilenameInitializerTests
    {
        private const string DefaultFilename = "DefaultFilename.txt";

        [Theory, AutoMockData]
        public void Throws_On_Null_Context(DefaultFilenameInitializerComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Sets_DefaultFilename_When_Possible(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            DefaultFilenameInitializerComponent sut)
        {
            // Arrange
            var template = new TestData.TemplateWithDefaultFilename(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), null, new StringBuilder(), DefaultFilename);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.DefaultFilename.Should().Be(DefaultFilename);
        }
    }
}
