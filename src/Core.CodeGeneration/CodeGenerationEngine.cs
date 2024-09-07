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

    public async Task Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(codeGenerationProvider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        await _templateProvider.StartSession(cancellationToken).ConfigureAwait(false);

        if (codeGenerationProvider is ITemplateComponentRegistryPlugin plugin)
        {
            await plugin.Initialize(_templateProvider, cancellationToken).ConfigureAwait(false);
        }

        var model = await codeGenerationProvider.CreateModel().ConfigureAwait(false);
        var additionalParameters = await codeGenerationProvider.CreateAdditionalParameters().ConfigureAwait(false);
        
        var result = await _templateEngine.Render(
            new RenderTemplateRequest
            (
                identifier: new TemplateTypeIdentifier(codeGenerationProvider.GetGeneratorType(), _templateFactory),
                model: model,
                generationEnvironment: generationEnvironment,
                additionalParameters: additionalParameters,
                defaultFilename: settings.DefaultFilename,
                context: null
            ), cancellationToken).ConfigureAwait(false);

        if (!result.IsSuccessful())
        {
            result.ThrowIfInvalid();
        }

        if (!settings.DryRun)
        {
            await generationEnvironment.SaveContents(codeGenerationProvider, settings.BasePath, settings.DefaultFilename, cancellationToken).ConfigureAwait(false);
        }
    }
}
