namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderTests
{
    protected Mock<IAssemblyService> AssemblyServiceMock { get; } = new();
    protected Provider CreateSut() => new(AssemblyServiceMock.Object);
}
