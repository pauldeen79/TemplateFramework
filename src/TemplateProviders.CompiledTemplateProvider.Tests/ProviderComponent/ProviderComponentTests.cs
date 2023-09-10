namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    protected IAssemblyService AssemblyServiceMock { get; } = Substitute.For<IAssemblyService>();
    protected ITemplateFactory CompiledTemplateFactoryMock { get; } = Substitute.For<ITemplateFactory>();

    protected ProviderComponent CreateSut() => new(AssemblyServiceMock, CompiledTemplateFactoryMock);
}
