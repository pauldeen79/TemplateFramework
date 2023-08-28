namespace TemplateFramework.Core.TemplateInitializerComponents;

public class DefaultFilenameInitializer : ITemplateInitializerComponent
{
    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        var templateType = request.Template.GetType();

        if (Array.Exists(templateType.GetInterfaces(), t => t.FullName?.Equals(typeof(IDefaultFilenameContainer).FullName, StringComparison.Ordinal) == true))
        {
            var defaultFilenameProperty = templateType.GetProperty(nameof(IDefaultFilenameContainer.DefaultFilename))!;
            defaultFilenameProperty.SetValue(request.Template, request.DefaultFilename);
        }
    }
}
