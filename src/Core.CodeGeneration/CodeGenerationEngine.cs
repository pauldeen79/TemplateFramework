namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationEngine : ICodeGenerationEngine
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

        _templateEngine.Render(new RenderTemplateRequest<object?>(
                               template: provider.CreateGenerator(),
                               generationEnvironment: generationEnvironment,
                               model: provider.CreateModel(),
                               defaultFilename: provider.DefaultFilename,
                               additionalParameters: provider.CreateAdditionalParameters(),
                               context: null));

        if (!settings.DryRun)
        {
            generationEnvironment.Process(provider, settings.BasePath);
        }
    }
}
