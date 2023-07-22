namespace TemplateFramework.Core.CodeGeneration;

public class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(ITemplateEngine templateEngine)
    {
        Guard.IsNotNull(templateEngine);

        _templateEngine = templateEngine;
    }

    private readonly ITemplateEngine _templateEngine;

    public void Generate(ICodeGenerationProvider provider, ITemplateFileManager templateFileManager, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNull(templateFileManager);
        Guard.IsNotNull(settings);

        var result = Initialize(provider, settings);
        if (provider.GenerateMultipleFiles)
        {
            _templateEngine.Render(new RenderTemplateRequest<object?>(
                                   template: result.generator,
                                   builder: templateFileManager.MultipleContentBuilder,
                                   model: provider.CreateModel(),
                                   defaultFilename: provider.DefaultFilename,
                                   additionalParameters: result.additionalParameters,
                                   context: null));
        }
        else
        {
            _templateEngine.Render(new RenderTemplateRequest<object?>(template: result.generator,
                                   builder: templateFileManager.StartNewFile(Path.Combine(provider.Path, provider.DefaultFilename)),
                                   model: provider.CreateModel(),
                                   defaultFilename: string.Empty,
                                   additionalParameters: result.additionalParameters,
                                   context: null));
        }

        ProcessResult(provider, result.shouldSave, result.shouldUseLastGeneratedFiles, templateFileManager);
    }

    private static (object generator, bool shouldSave, bool shouldUseLastGeneratedFiles, object? additionalParameters) Initialize(ICodeGenerationProvider provider, ICodeGenerationSettings settings)
    {
        provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);

        return
        (
            provider.CreateGenerator(),
            !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun,
            provider.GenerateMultipleFiles && !string.IsNullOrEmpty(provider.LastGeneratedFilesFilename),
            provider.CreateAdditionalParameters()
        );
    }

    private static void ProcessResult(ICodeGenerationProvider provider, bool shouldSave, bool shouldUseLastGeneratedFiles, ITemplateFileManager templateFileManager)
    {
        templateFileManager.Process(true, shouldSave);

        if (shouldSave)
        {
            if (shouldUseLastGeneratedFiles)
            {
                var prefixedLastGeneratedFilesFilename = Path.Combine(provider.Path, provider.LastGeneratedFilesFilename);
                templateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFilename, provider.RecurseOnDeleteGeneratedFiles);
                templateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFilename);
            }

            templateFileManager.SaveAll();
        }
    }
}
