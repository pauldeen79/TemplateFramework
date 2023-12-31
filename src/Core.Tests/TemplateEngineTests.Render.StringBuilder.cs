﻿namespace TemplateFramework.Core.Tests;

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
            sut.Invoking(x => x.Render(request: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Initializes_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            StringBuilder? builder = StringBuilder;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, builder);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);

            // Act
            sut.Render(request);

            // Assert
            TemplateInitializerMock.Received().Initialize(Arg.Any<ITemplateEngineContext>());
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new PlainTemplateWithAdditionalParameters();
            StringBuilder? generationEnvironment = StringBuilder;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, generationEnvironment);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);

            // Act
            sut.Render(request);

            // Assert
            TemplateRendererMock.Received().Render(Arg.Is<ITemplateEngineContext>(req =>
                req.Template == template
                && req.GenerationEnvironment.Type == GenerationEnvironmentType.StringBuilder
                && req.DefaultFilename == string.Empty));
        }
    }
}
