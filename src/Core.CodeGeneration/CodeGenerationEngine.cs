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

    public async Task Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(codeGenerationProvider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        _templateProvider.StartSession();

        var plugin = codeGenerationProvider as ITemplateComponentRegistryPlugin;
        plugin?.Initialize(_templateProvider);

        var model = await codeGenerationProvider.CreateModel().ConfigureAwait(false);
        var additionalParameters = await codeGenerationProvider.CreateAdditionalParameters().ConfigureAwait(false);
        
        _templateEngine.Render(
            new RenderTemplateRequest
            (
                identifier: new TemplateTypeIdentifier(codeGenerationProvider.GetGeneratorType(), _templateFactory),
                model: model,
                generationEnvironment: generationEnvironment,
                additionalParameters: additionalParameters,
                defaultFilename: settings.DefaultFilename,
                context: null
            ));

        if (!settings.DryRun)
        {
            await generationEnvironment.SaveContents(codeGenerationProvider, settings.BasePath, settings.DefaultFilename).ConfigureAwait(false);
        }
    }
}
