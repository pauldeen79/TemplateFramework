namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderTests
{
    protected Provider CreateSut() => new(new[] { TemplateCreatorMock.Object });
    protected Mock<ITemplateCreator> TemplateCreatorMock { get; } = new();
}
