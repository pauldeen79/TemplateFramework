namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Render : MultipleContentTemplateRendererTests
    {
        [Fact]
        public void Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(context: null!))
               .Should().Throw<ArgumentException>().WithParameterName("context");
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
            sut.Invoking(x => x.Render(engineContext))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Renders_MultipleContentBuilderTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = Substitute.For<IMultipleContentBuilderTemplate>();
            var generationEnvironment = Substitute.For<IMultipleContentBuilder>();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(templateMock), DefaultFilename, generationEnvironment);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock, TemplateProviderMock, templateMock);
            MultipleContentBuilderTemplateCreatorMock.TryCreate(Arg.Any<object>()).Returns(templateMock);

            // Act
            sut.Render(engineContext);

            // Assert
            templateMock.Received().Render(Arg.Any<IMultipleContentBuilder>());
        }

        [Fact]
        public void Renders_Non_MultipleContentBuilderTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = Substitute.For<IMultipleContentBuilder>();
            var contentBuilderMock = Substitute.For<IContentBuilder>();
            MultipleContentBuilderTemplateCreatorMock.TryCreate(Arg.Any<object>()).Returns(default(IMultipleContentBuilderTemplate));
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
                .When(x => x.Render(Arg.Any<IRenderTemplateRequest>()))
                .Do(x => ((StringBuilderEnvironment)x.ArgAt<IRenderTemplateRequest>(0).GenerationEnvironment).Builder.Append(template.ToString()));

            // Act
            sut.Render(engineContext);

            // Assert
            contentBuilderMock.Builder.Should().NotBeNull();
            contentBuilderMock.Builder.ToString().Should().Be("TemplateFramework.Core.Tests.TestData+TextTransformTemplate");
        }
    }
}
