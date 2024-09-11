using CrossCutting.Common.Results;

namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleStringContentBuilderTemplateRendererTests
{
    public class Render : MultipleStringContentBuilderTemplateRendererTests
    {
        [Fact]
        public void Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.Render(context: null!, CancellationToken.None))
               .Should().ThrowAsync<ArgumentException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_When_GenerationEnvironemnt_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), DefaultFilename, new StringBuilder());
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);

            // Act & Assert
            sut.Awaiting(x => x.Render(engineContext, CancellationToken.None))
               .Should().ThrowAsync<NotSupportedException>();
        }

        [Fact]
        public async Task Renders_MultipleContentBuilderTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = Substitute.For<IMultipleContentBuilderTemplate<StringBuilder>>();
            var generationEnvironment = Substitute.For<IMultipleContentBuilder<StringBuilder>>();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(templateMock), DefaultFilename, generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, templateMock);
            MultipleContentBuilderTemplateCreatorMock.TryCreate(Arg.Any<object>()).Returns(templateMock);

            // Act
            await sut.Render(engineContext, CancellationToken.None);

            // Assert
            await templateMock.Received().Render(Arg.Any<IMultipleContentBuilder<StringBuilder>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Renders_Non_MultipleContentBuilderTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = Substitute.For<IMultipleContentBuilder<StringBuilder>>();
            var contentBuilderMock = Substitute.For<IContentBuilder<StringBuilder>>();
            MultipleContentBuilderTemplateCreatorMock.TryCreate(Arg.Any<object>()).Returns(default(IMultipleContentBuilderTemplate<StringBuilder>));
            generationEnvironment.AddContent(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<StringBuilder?>())
                                 .Returns(x =>
                                 {
                                     contentBuilderMock.Builder.Returns(x.ArgAt<StringBuilder?>(2) ?? new StringBuilder());

                                     return contentBuilderMock;
                                 });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), DefaultFilename, generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(template);
            TemplateEngineMock
                .Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>())
                .Returns(x =>
                {
                    ((StringBuilderEnvironment)x.ArgAt<IRenderTemplateRequest>(0).GenerationEnvironment).Builder.Append(template.ToString());
                    return Result.Success();
                });

            // Act
            await sut.Render(engineContext, CancellationToken.None);

            // Assert
            contentBuilderMock.Builder.Should().NotBeNull();
            contentBuilderMock.Builder.ToString().Should().Be("TemplateFramework.Core.Tests.TestData+TextTransformTemplate");
        }

        [Fact]
        public async Task Returns_Success_When_Rendering_Is_Succesful()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = Substitute.For<IMultipleContentBuilder<StringBuilder>>();
            var contentBuilderMock = Substitute.For<IContentBuilder<StringBuilder>>();
            MultipleContentBuilderTemplateCreatorMock.TryCreate(Arg.Any<object>()).Returns(default(IMultipleContentBuilderTemplate<StringBuilder>));
            generationEnvironment.AddContent(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<StringBuilder?>())
                                 .Returns(x =>
                                 {
                                     contentBuilderMock.Builder.Returns(x.ArgAt<StringBuilder?>(2) ?? new StringBuilder());

                                     return contentBuilderMock;
                                 });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), DefaultFilename, generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(template);
            TemplateEngineMock
                .Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>())
                .Returns(x =>
                {
                    ((StringBuilderEnvironment)x.ArgAt<IRenderTemplateRequest>(0).GenerationEnvironment).Builder.Append(template.ToString());
                    return Result.Success();
                });

            // Act
            var result = await sut.Render(engineContext, CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Rendering_Result_When_Rendering_Is_Not_Succesful()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = Substitute.For<IMultipleContentBuilder<StringBuilder>>();
            var contentBuilderMock = Substitute.For<IContentBuilder<StringBuilder>>();
            MultipleContentBuilderTemplateCreatorMock.TryCreate(Arg.Any<object>()).Returns(default(IMultipleContentBuilderTemplate<StringBuilder>));
            generationEnvironment.AddContent(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<StringBuilder?>())
                                 .Returns(x =>
                                 {
                                     contentBuilderMock.Builder.Returns(x.ArgAt<StringBuilder?>(2) ?? new StringBuilder());

                                     return contentBuilderMock;
                                 });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), DefaultFilename, generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, template);
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(template);
            TemplateEngineMock
                .Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>())
                .Returns(x =>
                {
                    ((StringBuilderEnvironment)x.ArgAt<IRenderTemplateRequest>(0).GenerationEnvironment).Builder.Append(template.ToString());
                    return Result.Error();
                });

            // Act
            var result = await sut.Render(engineContext, CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
        }
    }
}
