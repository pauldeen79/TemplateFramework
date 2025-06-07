namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_General : TemplateEngineTests
    {
        public Render_General()
        {
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(true);
        }

        [Fact]
        public async Task Returns_Error_When_TemplateProvider_Does_Not_Create_Template_Instance()
        {
            // Arrange
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(default(object));
            var sut = CreateSut();
            var request = Substitute.For<IRenderTemplateRequest>();

            // Act
            var result = await sut.RenderAsync(request, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("TemplateProvider did not create a template instance");
        }

        [Fact]
        public async Task Returns_Result_From_Initializer_When_Initialization_Returns_Not_Successful()
        {
            // Arrange
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(new object());
            TemplateInitializerMock.Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Error("Kaboom"));
            var sut = CreateSut();
            var request = Substitute.For<IRenderTemplateRequest>();

            // Act
            var result = await sut.RenderAsync(request, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_NotSupported_When_No_Renderer_Supports_The_GenerationEnvironment()
        {
            // Arrange
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(new object());
            TemplateInitializerMock.Initialize(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            TemplateRendererMock.Supports(Arg.Any<IGenerationEnvironment>()).Returns(false);
            var sut = CreateSut();
            var request = Substitute.For<IRenderTemplateRequest>();

            // Act
            var result = await sut.RenderAsync(request, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldStartWith("Type of GenerationEnvironment");
            result.ErrorMessage.ShouldEndWith("is not supported");
        }
    }
}
