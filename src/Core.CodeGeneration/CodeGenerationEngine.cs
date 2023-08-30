namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(ITemplateEngine templateEngine)
    {
        Guard.IsNotNull(templateEngine);

        _templateEngine = templateEngine;
    }

    private readonly ITemplateEngine _templateEngine;

    public void Generate(ICodeGenerationProvider provider, IGenerationEnvironment generationEnvironment, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        _templateEngine.Render(
            new RenderTemplateRequest
            (
                identifier: new TemplateInstanceIdentifier(provider.CreateGenerator()),
                model: provider.CreateModel(),
                generationEnvironment: generationEnvironment,
                additionalParameters: provider.CreateAdditionalParameters(),
                defaultFilename: settings.DefaultFilename,
                context: null
            ));

        if (!settings.DryRun)
        {
            generationEnvironment.SaveContents(provider, settings.BasePath, settings.DefaultFilename);
        }
    }
}
