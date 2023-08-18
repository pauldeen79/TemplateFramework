namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    protected Mock<IAssemblyService> AssemblyServiceMock { get; } = new();
    protected ProviderComponent CreateSut() => new(AssemblyServiceMock.Object);
}
