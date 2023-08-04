namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    protected Mock<IMultipleContentBuilderTemplateCreator> MultipleContentBuilderTemplateCreatorMock { get; } = new();

    protected MultipleContentTemplateRenderer CreateSut() => new(new[] { MultipleContentBuilderTemplateCreatorMock.Object });

    protected const string DefaultFilename = "MyFile.txt";
}
