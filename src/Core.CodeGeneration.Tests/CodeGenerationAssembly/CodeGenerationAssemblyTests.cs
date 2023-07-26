namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    protected Mock<ICodeGenerationEngine> CodeGenerationEngineMock { get; } = new();
    protected Mock<IGenerationEnvironment> GenerationEnvironmentMock { get; } = new();

    protected CodeGenerationAssembly CreateSut() => new(CodeGenerationEngineMock.Object);
}
