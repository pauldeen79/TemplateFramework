namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilder : TemplateEngineTests
    {
        public Render_MultipleContentBuilder()
        {
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
        }

        [Fact]
        public async Task Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.Render(request: null!, CancellationToken.None))
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("request");
        }

        [Fact]
        public async Task Initializes_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder<StringBuilder>? generationEnvironment = MultipleContentBuilderMock;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment, additionalParameters);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);
            TemplateInitializerMock.Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await sut.Render(request, CancellationToken.None);

            // Assert
            await TemplateInitializerMock.Received().Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder<StringBuilder>? generationEnvironment = MultipleContentBuilderMock;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, generationEnvironment);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);
            TemplateInitializerMock.Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await sut.Render(request, CancellationToken.None);

            // Assert
            await TemplateRendererMock.Received().Render(Arg.Is<ITemplateEngineContext>(req =>
                req.Identifier is TemplateInstanceIdentifier
                && req.GenerationEnvironment is MultipleContentBuilderEnvironment<StringBuilder>
                && req.DefaultFilename == string.Empty), Arg.Any<CancellationToken>());
        }
    }
}
