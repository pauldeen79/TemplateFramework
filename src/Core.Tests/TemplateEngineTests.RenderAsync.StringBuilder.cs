﻿namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class RenderAsync_StringBuilder : TemplateEngineTests
    {
        public RenderAsync_StringBuilder()
        {
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
        }

        [Fact]
        public async Task Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.RenderAsync(request: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("request");
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
            TemplateInitializerMock.InitializeAsync(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await sut.RenderAsync(request, CancellationToken.None);

            // Assert
            await TemplateInitializerMock.Received().InitializeAsync(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            StringBuilder? generationEnvironment = StringBuilder;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, generationEnvironment);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);
            TemplateInitializerMock.InitializeAsync(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await sut.RenderAsync(request, CancellationToken.None);

            // Assert
            await TemplateRendererMock.Received().RenderAsync(Arg.Is<ITemplateEngineContext>(req =>
                req.Template == template
                && req.GenerationEnvironment is StringBuilderEnvironment
                && req.DefaultFilename == string.Empty), Arg.Any<CancellationToken>());
        }
    }
}
