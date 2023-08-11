namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    protected Mock<ISingleContentTemplateRenderer> SingleContentTemplateRendererMock { get; } = new();
    protected Mock<IMultipleContentBuilderTemplateCreator> MultipleContentBuilderTemplateCreatorMock { get; } = new();

    protected MultipleContentTemplateRenderer CreateSut() => new(SingleContentTemplateRendererMock.Object, new[] { MultipleContentBuilderTemplateCreatorMock.Object });

    protected const string DefaultFilename = "MyFile.txt";
}
