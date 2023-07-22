namespace TemplateFramework.Core.Tests;

public partial class TemplateFactoryTests
{
    protected TemplateFactory CreateSut() => new(new[] { TemplateCreatorMock.Object });
    protected Mock<ITemplateCreator> TemplateCreatorMock { get; } = new();
}
