namespace TemplateFramework.Core.TemplateInitializerComponents;

public class DefaultFilenameInitializerComponent : ITemplateInitializerComponent
{
    public Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        var templateType = context.Template.GetType();

        if (Array.Exists(templateType.GetInterfaces(), t => t.FullName?.Equals(typeof(IDefaultFilenameContainer).FullName, StringComparison.Ordinal) == true))
        {
            var defaultFilenameProperty = templateType.GetProperty(nameof(IDefaultFilenameContainer.DefaultFilename))!;
            defaultFilenameProperty.SetValue(context.Template, context.DefaultFilename);
        }

        return Task.CompletedTask;
    }
}
