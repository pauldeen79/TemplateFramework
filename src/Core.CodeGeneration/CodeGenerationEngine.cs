namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(ITemplateEngine templateEngine)
    {
        Guard.IsNotNull(templateEngine);

        _templateEngine = templateEngine;
    }

    private readonly ITemplateEngine _templateEngine;

    public void Generate(ICodeGenerationProvider provider, IMultipleContentBuilder generationEnvironment, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(settings);

        provider.Initialize(settings.SkipWhenFileExists);

        _templateEngine.Render(new RenderTemplateRequest<object?>(
                               template: provider.CreateGenerator(),
                               builder: generationEnvironment,
                               model: provider.CreateModel(),
                               defaultFilename: provider.DefaultFilename,
                               additionalParameters: provider.CreateAdditionalParameters(),
                               context: null));

        if (settings.DryRun)
        {
            return;
        }

        if (!string.IsNullOrEmpty(provider.LastGeneratedFilesFilename))
        {
            var prefixedLastGeneratedFilesFilename = Path.Combine(provider.Path, provider.LastGeneratedFilesFilename);
            generationEnvironment.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFilename, provider.RecurseOnDeleteGeneratedFiles);
            generationEnvironment.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFilename);
        }

        generationEnvironment.SaveAll();
    }
}
