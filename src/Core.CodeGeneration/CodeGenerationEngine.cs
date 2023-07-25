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

        var result = Initialize(provider, settings);

        _templateEngine.Render(new RenderTemplateRequest<object?>(
                                template: result.generator,
                                builder: generationEnvironment,
                                model: provider.CreateModel(),
                                defaultFilename: provider.DefaultFilename,
                                additionalParameters: result.additionalParameters,
                                context: null));
        
        ProcessResult(provider, result.shouldSave, result.shouldUseLastGeneratedFiles, generationEnvironment);
    }

    private static (object generator, bool shouldSave, bool shouldUseLastGeneratedFiles, object? additionalParameters) Initialize(ICodeGenerationProvider provider, ICodeGenerationSettings settings)
    {
        provider.Initialize(settings.SkipWhenFileExists);

        return
        (
            provider.CreateGenerator(),
            !settings.DryRun,
            !string.IsNullOrEmpty(provider.LastGeneratedFilesFilename),
            provider.CreateAdditionalParameters()
        );
    }

    private static void ProcessResult(ICodeGenerationProvider provider, bool shouldSave, bool shouldUseLastGeneratedFiles, IMultipleContentBuilder generationEnvironment)
    {
        if (shouldSave)
        {
            if (shouldUseLastGeneratedFiles)
            {
                var prefixedLastGeneratedFilesFilename = Path.Combine(provider.Path, provider.LastGeneratedFilesFilename);
                generationEnvironment.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFilename, provider.RecurseOnDeleteGeneratedFiles);
                generationEnvironment.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFilename);
            }

            generationEnvironment.SaveAll();
        }
    }
}
