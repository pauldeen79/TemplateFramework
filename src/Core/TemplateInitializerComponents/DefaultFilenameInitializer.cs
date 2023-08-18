namespace TemplateFramework.Core.TemplateInitializerComponents;

public class DefaultFilenameInitializer : ITemplateInitializerComponent
{
    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        if (request.Template is not IDefaultFilenameContainer defaultFilenameContainer)
        {
            return;
        }

        defaultFilenameContainer.DefaultFilename = request.DefaultFilename;
    }
}
