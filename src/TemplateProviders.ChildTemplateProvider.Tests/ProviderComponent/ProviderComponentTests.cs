namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    protected ProviderComponent CreateSut() => new(new[] { TemplateCreatorMock.Object });
    protected Mock<ITemplateCreator> TemplateCreatorMock { get; } = new();
}
