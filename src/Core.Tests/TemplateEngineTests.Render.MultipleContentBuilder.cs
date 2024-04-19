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
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment, additionalParameters);
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
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), additionalParameters, generationEnvironment);
            TemplateProviderMock.Create(Arg.Any<TemplateInstanceIdentifier>()).Returns(template);

            // Act
            sut.Render(request);

            // Assert
            TemplateRendererMock.Received().Render(Arg.Is<ITemplateEngineContext>(req =>
                req.Identifier is TemplateInstanceIdentifier
                && req.GenerationEnvironment.Type == GenerationEnvironmentType.MultipleContentBuilder
                && req.DefaultFilename == string.Empty));
        }
    }
}
