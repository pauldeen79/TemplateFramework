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

    public void Generate(ICodeGenerationProvider codeGenerationProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(codeGenerationProvider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        _templateProvider.StartSession();

        var plugin = codeGenerationProvider as ITemplateProviderPlugin;
        plugin?.Initialize(_templateProvider);

        _templateEngine.Render(
            new RenderTemplateRequest
            (
                identifier: new TemplateTypeIdentifier(codeGenerationProvider.GetGeneratorType(), _templateFactory),
                model: codeGenerationProvider.CreateModel(),
                generationEnvironment: generationEnvironment,
                additionalParameters: codeGenerationProvider.CreateAdditionalParameters(),
                defaultFilename: settings.DefaultFilename,
                context: null
            ));

        if (!settings.DryRun)
        {
            generationEnvironment.SaveContents(codeGenerationProvider, settings.BasePath, settings.DefaultFilename);
        }
    }
}
