namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ModelInitializerComponent : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ModelInitializerComponent(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public int Order => 1;

    public Task<Result> InitializeAsync(ITemplateEngineContext context, CancellationToken token)
        => Task.Run(() =>
        {
            Guard.IsNotNull(context);
            Guard.IsNotNull(context.Template);

            var templateType = context.Template.GetType();

            if (!Array.Exists(templateType.GetInterfaces(), t => t.FullName?.StartsWith("TemplateFramework.Abstractions.IModelContainer", StringComparison.InvariantCulture) == true))
            {
                return Result.Continue();
            }

            var modelProperty = templateType.GetProperty(nameof(IModelContainer<object?>.Model))!;
            return _converter.Convert(context.Model, modelProperty.PropertyType, context)
                .OnSuccess(result => modelProperty.SetValue(context.Template, result.Value));
        }, token);
}
