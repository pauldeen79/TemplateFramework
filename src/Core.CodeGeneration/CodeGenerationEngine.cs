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

        var request = provider.CreateRequest(generationEnvironment);
        _templateEngine.Render(request);

        if (!settings.DryRun)
        {
            generationEnvironment.SaveContents(provider, settings.BasePath, request.DefaultFilename);
        }
    }
}
