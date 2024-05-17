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
            sut.Awaiting(x => x.Initialize(context: null!, CancellationToken.None))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public async Task Sets_DefaultFilename_When_Possible(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            DefaultFilenameInitializerComponent sut)
        {
            // Arrange
            var template = new TestData.TemplateWithDefaultFilename(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), null, new StringBuilder(), DefaultFilename);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            await sut.Initialize(engineContext, CancellationToken.None);

            // Assert
            template.DefaultFilename.Should().Be(DefaultFilename);
        }
    }
}
