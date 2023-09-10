namespace TemplateFramework.Core.CodeGeneration.Tests;

public abstract partial class CodeGenerationEngineTests
{
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateFactory TemplateFactoryMock { get; } = Substitute.For<ITemplateFactory>();
    protected ICodeGenerationProvider CodeGenerationProviderMock { get; } = Substitute.For<ICodeGenerationProvider>();
    protected ICodeGenerationSettings CodeGenerationSettingsMock { get; } = Substitute.For<ICodeGenerationSettings>();
    protected IGenerationEnvironment GenerationEnvironmentMock { get; } = Substitute.For<IGenerationEnvironment>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();

    protected CodeGenerationEngine CreateSut() => new(TemplateEngineMock, TemplateFactoryMock, TemplateProviderMock);
}
