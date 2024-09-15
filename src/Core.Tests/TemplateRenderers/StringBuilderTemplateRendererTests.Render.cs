namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    public class Render : StringBuilderTemplateRendererTests
    {
        public Render()
        {
            StringBuilderTemplateRendererMock.TryRender(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        [Fact]
        public async Task Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            await sut.Awaiting(x => x.Render(context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public async Task Throws_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), Substitute.For<IMultipleContentBuilder<StringBuilder>>());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act & Assert
            await sut.Awaiting(x => x.Render(engineContext, CancellationToken.None))
                     .Should().ThrowAsync<NotSupportedException>();
        }

        [Fact]
        public async Task Renders_Template_Correctly_When_A_TemplateRenderer_Supports_The_Template()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            await sut.Render(engineContext, CancellationToken.None);

            // Assert
            await StringBuilderTemplateRendererMock.Received().TryRender(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Renders_Template_Using_ToString_When_No_TemplateRenderer_Supports_The_Template()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            StringBuilderTemplateRendererMock.TryRender(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>())
                .Returns(Result.Continue());

            // Act
            await sut.Render(engineContext, CancellationToken.None);

            // Assert
            generationEnvironment.ToString().Should().Be("TemplateFramework.Core.Tests.TestData+Template");
        }

        [Fact]
        public async Task Returns_Success_When_A_TemplateRenderer_Supports_The_Template_And_Rendering_Succeeds()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            StringBuilderTemplateRendererMock.TryRender(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            var result = await sut.Render(engineContext, CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Rendering_Result_When_A_TemplateRenderer_Supports_The_Template_And_Rendering_Does_Not_Succeed()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            StringBuilderTemplateRendererMock.TryRender(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>()).Returns(Result.Error());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            var result = await sut.Render(engineContext, CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
        }
    }
}
