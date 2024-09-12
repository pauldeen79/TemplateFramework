namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_StringBuilder : TemplateEngineTests
    {
        public Render_StringBuilder()
        {
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
        }

        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.Render(request: null!, CancellationToken.None))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public async Task Initializes_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            StringBuilder? builder = StringBuilder;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, builder);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);
            TemplateInitializerMock.Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await sut.Render(request, CancellationToken.None);

            // Assert
            await TemplateInitializerMock.Received().Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async  Task Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            StringBuilder? generationEnvironment = StringBuilder;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, generationEnvironment);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);
            TemplateInitializerMock.Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await sut.Render(request, CancellationToken.None);

            // Assert
            await TemplateRendererMock.Received().Render(Arg.Is<ITemplateEngineContext>(req =>
                req.Template == template
                && req.GenerationEnvironment is StringBuilderEnvironment
                && req.DefaultFilename == string.Empty), Arg.Any<CancellationToken>());
        }
    }
}
