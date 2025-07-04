﻿namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class StringBuilderTemplateRendererTests
{
    public class RenderAsync : StringBuilderTemplateRendererTests
    {
        public RenderAsync()
        {
            StringBuilderTemplateRendererMock.TryRenderAsync(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        [Fact]
        public async Task Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.RenderAsync(context: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("context");
        }

        [Fact]
        public async Task Returns_NotSupported_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), Substitute.For<IMultipleContentBuilder<StringBuilder>>());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            var result = await sut.RenderAsync(engineContext, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
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
            await sut.RenderAsync(engineContext, CancellationToken.None);

            // Assert
            await StringBuilderTemplateRendererMock.Received().TryRenderAsync(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>());
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
            StringBuilderTemplateRendererMock.TryRenderAsync(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>())
                .Returns(Result.Continue());

            // Act
            await sut.RenderAsync(engineContext, CancellationToken.None);

            // Assert
            generationEnvironment.ToString().ShouldBe("TemplateFramework.Core.Tests.TestData+Template");
        }

        [Fact]
        public async Task Returns_Success_When_A_TemplateRenderer_Supports_The_Template_And_Rendering_Succeeds()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            StringBuilderTemplateRendererMock.TryRenderAsync(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            var result = await sut.RenderAsync(engineContext, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Rendering_Result_When_A_TemplateRenderer_Supports_The_Template_And_Rendering_Does_Not_Succeed()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), generationEnvironment);
            StringBuilderTemplateRendererMock.TryRenderAsync(Arg.Any<object>(), Arg.Any<StringBuilder>(), Arg.Any<CancellationToken>()).Returns(Result.Error());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act
            var result = await sut.RenderAsync(engineContext, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }
    }
}
