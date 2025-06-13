namespace TemplateFramework.Core.CodeGeneration;

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

    public async Task<Result> GenerateAsync(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(codeGenerationProvider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        var results = await new AsyncResultDictionaryBuilder()
            .Add(nameof(ITemplateProvider.StartSessionAsync), _templateProvider.StartSessionAsync(cancellationToken))
            .Add(nameof(ITemplateComponentRegistryPlugin.InitializeAsync), codeGenerationProvider is ITemplateComponentRegistryPlugin x
                ? x.InitializeAsync(_templateProvider, cancellationToken)
                : Task.FromResult(Result.Continue()))
            .Add(nameof(ICodeGenerationProvider.CreateModelAsync), codeGenerationProvider.CreateModelAsync(cancellationToken))
            .Add(nameof(ICodeGenerationProvider.CreateAdditionalParametersAsync), codeGenerationProvider.CreateAdditionalParametersAsync(cancellationToken))
            .Build()
            .ConfigureAwait(false);

        // Note that there will be an extension method on Dictionary<string, Result> to get the error in the near future
        var error = results.GetError();
        if (error is not null)
        {
            // Error in initialization
            return error;
        }

        var modelResult = results[nameof(ICodeGenerationProvider.CreateModelAsync)];
        var additionalParametersResult = results[nameof(ICodeGenerationProvider.CreateAdditionalParametersAsync)];

        var result = await _templateEngine.RenderAsync(
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
            return await generationEnvironment.SaveContentsAsync(codeGenerationProvider, settings.BasePath, settings.DefaultFilename, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }
}
