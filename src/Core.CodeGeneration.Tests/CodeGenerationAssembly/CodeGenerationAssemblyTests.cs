namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    protected ICodeGenerationEngine CodeGenerationEngineMock { get; } = Substitute.For<ICodeGenerationEngine>();
    protected IAssemblyService AssemblyServiceMock { get; } = Substitute.For<IAssemblyService>();
    protected IGenerationEnvironment GenerationEnvironmentMock { get; } = Substitute.For<IGenerationEnvironment>();
    protected ICodeGenerationProviderCreator CodeGenerationProviderCreatorMock { get; } = Substitute.For<ICodeGenerationProviderCreator>();

    protected CodeGenerationAssembly CreateSut() => new(CodeGenerationEngineMock, AssemblyServiceMock, new[] { CodeGenerationProviderCreatorMock });
}
