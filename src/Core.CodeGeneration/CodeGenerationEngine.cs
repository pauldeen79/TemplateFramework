﻿namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(ITemplateEngine templateEngine, ITemplateFactory templateFactory, ITemplateProvider templateProvider)
    {
        Guard.IsNotNull(templateEngine);
        Guard.IsNotNull(templateFactory);
        Guard.IsNotNull(templateProvider);

        _templateEngine = templateEngine;
        _templateFactory = templateFactory;
        _templateProvider = templateProvider;
    }

    private readonly ITemplateEngine _templateEngine;
    private readonly ITemplateFactory _templateFactory;
    private readonly ITemplateProvider _templateProvider;

    public async Task<Result> Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(codeGenerationProvider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        var resultSetBuilder = new ResultDictionaryBuilder();
        resultSetBuilder.Add(nameof(ITemplateProvider.StartSession), () => _templateProvider.StartSession(cancellationToken));
        if (codeGenerationProvider is ITemplateComponentRegistryPlugin plugin)
        {
            resultSetBuilder.Add(nameof(ITemplateComponentRegistryPlugin.Initialize), () => plugin.Initialize(_templateProvider, cancellationToken));
        }
        resultSetBuilder.Add(nameof(ICodeGenerationProvider.CreateModel), () => codeGenerationProvider.CreateModel(cancellationToken));
        resultSetBuilder.Add(nameof(ICodeGenerationProvider.CreateAdditionalParameters), () => codeGenerationProvider.CreateAdditionalParameters(cancellationToken));

        var results = await resultSetBuilder.Build().ConfigureAwait(false);

        var error = results
            .Select(x => new { x.Key, Result = x.Value })
            .FirstOrDefault(x => !x.Result.IsSuccessful());
        if (error is not null)
        {
            // Error in initialization
            return error.Result;
        }

        var modelResult = results[nameof(ICodeGenerationProvider.CreateModel)];
        var additionalParametersResult = results[nameof(ICodeGenerationProvider.CreateAdditionalParameters)];

        var result = await _templateEngine.Render(
            new RenderTemplateRequest
            (
                identifier: new TemplateTypeIdentifier(codeGenerationProvider.GetGeneratorType(), _templateFactory),
                model: modelResult.GetValue(),
                generationEnvironment: generationEnvironment,
                additionalParameters: additionalParametersResult.GetValue(),
                defaultFilename: settings.DefaultFilename,
                context: null
            ), cancellationToken).ConfigureAwait(false);

        if (!result.IsSuccessful())
        {
            return result;
        }

        if (!settings.DryRun)
        {
            return await generationEnvironment.SaveContents(codeGenerationProvider, settings.BasePath, settings.DefaultFilename, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }
}
