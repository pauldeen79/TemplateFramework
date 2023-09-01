namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    protected Mock<IAssemblyService> AssemblyServiceMock { get; } = new();
    protected Mock<ICompiledTemplateFactory> CompiledTemplateFactoryMock { get; } = new();

    protected ProviderComponent CreateSut() => new(AssemblyServiceMock.Object, CompiledTemplateFactoryMock.Object);
}
