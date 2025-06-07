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

    public async Task<Result> Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(codeGenerationProvider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        var results = await new AsyncResultDictionaryBuilder()
            .Add(nameof(ITemplateProvider.StartSession), _templateProvider.StartSession(cancellationToken))
            .Add(nameof(ITemplateComponentRegistryPlugin.Initialize), codeGenerationProvider is ITemplateComponentRegistryPlugin x
                ? x.Initialize(_templateProvider, cancellationToken)
                : Task.FromResult(Result.Continue()))
            .Add(nameof(ICodeGenerationProvider.CreateModel), codeGenerationProvider.CreateModel(cancellationToken))
            .Add(nameof(ICodeGenerationProvider.CreateAdditionalParameters), codeGenerationProvider.CreateAdditionalParameters(cancellationToken))
            .Build()
            .ConfigureAwait(false);

        // Note that there will be an extension method on Dictionary<string, Result> to get the error in the near future
        var error = results.GetError();
        if (error is not null)
        {
            // Error in initialization
            return error;
        }

        var modelResult = results[nameof(ICodeGenerationProvider.CreateModel)];
        var additionalParametersResult = results[nameof(ICodeGenerationProvider.CreateAdditionalParameters)];

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
            return await generationEnvironment.SaveContents(codeGenerationProvider, settings.BasePath, settings.DefaultFilename, cancellationToken).ConfigureAwait(false);
        }

        return result;
    }
}
