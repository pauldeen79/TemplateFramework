namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    protected Mock<ICodeGenerationEngine> CodeGenerationEngineMock { get; } = new();
    protected Mock<ITemplateFileManagerFactory> TemplateFileManagerFactoryMock { get; } = new();

    protected CodeGenerationAssembly CreateSut() => new(CodeGenerationEngineMock.Object, TemplateFileManagerFactoryMock.Object);
}
