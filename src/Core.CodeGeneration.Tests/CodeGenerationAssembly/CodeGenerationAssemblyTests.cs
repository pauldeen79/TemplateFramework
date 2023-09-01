namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    protected Mock<ICodeGenerationEngine> CodeGenerationEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    protected Mock<IAssemblyService> AssemblyServiceMock { get; } = new();
    protected Mock<IGenerationEnvironment> GenerationEnvironmentMock { get; } = new();
    protected Mock<ICodeGenerationProviderCreator> CodeGenerationProviderCreatorMock { get; } = new();

    protected CodeGenerationAssembly CreateSut() => new(CodeGenerationEngineMock.Object, TemplateProviderMock.Object, AssemblyServiceMock.Object, new[] { CodeGenerationProviderCreatorMock.Object });
}
