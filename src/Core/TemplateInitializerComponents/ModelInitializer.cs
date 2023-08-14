namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ModelInitializer : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ModelInitializer(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        var templateType = request.Template.GetType();
        
        if (Array.Exists(templateType.GetInterfaces(), t => t.FullName?.StartsWith("TemplateFramework.Abstractions.IModelContainer", StringComparison.InvariantCulture) == true))
        {
            var modelProperty = templateType.GetProperty(nameof(IModelContainer<object?>.Model))!;
            modelProperty.SetValue(request.Template, _converter.Convert(request.Model, modelProperty.PropertyType));
        }
    }
}
