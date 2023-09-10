namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    protected ProviderComponent CreateSut() => new(new[] { TemplateCreatorMock });
    protected ITemplateCreator TemplateCreatorMock { get; } = Substitute.For<ITemplateCreator>();
}
