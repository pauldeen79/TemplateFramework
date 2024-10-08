﻿namespace TemplateFramework.Abstractions.Extensions;

public static class CodeGenerationEngineExtensions
{
    public static Task<Result> Generate(this ICodeGenerationEngine instance, ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings)
        => instance.Generate(codeGenerationProvider, generationEnvironment, settings, CancellationToken.None);
}
