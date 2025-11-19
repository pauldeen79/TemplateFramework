namespace TemplateFramework.Core.TemplateInitializerComponents;

public class DefaultFilenameInitializerComponent : ITemplateInitializerComponent
{
    public int Order => 2;

    public Task<Result> InitializeAsync(ITemplateEngineContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        var templateType = context.Template.GetType();

        if (!Array.Exists(templateType.GetInterfaces(), t => t.FullName?.Equals(typeof(IDefaultFilenameContainer).FullName, StringComparison.Ordinal) == true))
        {
            return Task.FromResult(Result.Continue());
        }

        var defaultFilenameProperty = templateType.GetProperty(nameof(IDefaultFilenameContainer.DefaultFilename))!;
        defaultFilenameProperty.SetValue(context.Template, context.DefaultFilename);
        return Task.FromResult(Result.Success());
    }
}
