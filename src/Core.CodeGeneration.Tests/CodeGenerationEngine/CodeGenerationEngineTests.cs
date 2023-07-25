﻿namespace TemplateFramework.Core.CodeGeneration.Tests;

public abstract partial class CodeGenerationEngineTests
{
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ICodeGenerationProvider> CodeGenerationProviderMock { get; } = new();
    protected Mock<ICodeGenerationSettings> CodeGenerationSettingsMock { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();

    protected CodeGenerationEngine CreateSut() => new(TemplateEngineMock.Object);
}
