﻿namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    Task<Result> Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings, CancellationToken cancellationToken);
}
