namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(ITemplateEngine templateEngine)
    {
        Guard.IsNotNull(templateEngine);

        _templateEngine = templateEngine;
    }

    private readonly ITemplateEngine _templateEngine;

    public void Generate(ICodeGenerationProvider codeGenerationProvider, ITemplateProvider templateProvider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(codeGenerationProvider);
        Guard.IsNotNull(templateProvider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        var plugin = codeGenerationProvider as ITemplateProviderPlugin;
        plugin?.Initialize(templateProvider);

        _templateEngine.Render(
            new RenderTemplateRequest
            (
                identifier: new TemplateInstanceIdentifier(codeGenerationProvider.CreateGenerator()),
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
