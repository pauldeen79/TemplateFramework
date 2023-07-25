namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    protected Mock<ICodeGenerationEngine> CodeGenerationEngineMock { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleConentBuilderMock { get; } = new();

    protected CodeGenerationAssembly CreateSut() => new(CodeGenerationEngineMock.Object);
}
