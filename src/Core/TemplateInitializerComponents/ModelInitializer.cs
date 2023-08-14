namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ModelInitializer : ITemplateInitializerComponent
{
    private readonly IEnumerable<ITemplateParameterConverter> _converters;

    public ModelInitializer(IEnumerable<ITemplateParameterConverter> converters)
    {
        Guard.IsNotNull(converters);

        _converters = converters;
    }

    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        var templateType = request.Template.GetType();
        
        if (Array.Exists(templateType.GetInterfaces(), t => t.FullName?.StartsWith("TemplateFramework.Abstractions.IModelContainer", StringComparison.InvariantCulture) == true))
        {
            var modelProperty = templateType.GetProperty(nameof(IModelContainer<object?>.Model))!;
            modelProperty.SetValue(request.Template, ConvertType(request.Model, modelProperty.PropertyType));
        }
    }

    private object? ConvertType(object? value, Type type)
    {
        foreach (var converter in _converters)
        {
            if (converter.TryConvert(value, type, out var convertedValue))
            {
                return convertedValue;
            }
        }

        return value;
    }
}
