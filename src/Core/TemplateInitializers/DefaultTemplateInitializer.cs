namespace TemplateFramework.Core.TemplateInitializers;

public sealed class DefaultTemplateInitializer : ITemplateInitializer
{
    private readonly IEnumerable<ITemplateParameterConverter> _converters;

    public DefaultTemplateInitializer(IEnumerable<ITemplateParameterConverter> converters)
    {
        Guard.IsNotNull(converters);

        _converters = converters;
    }

    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        TrySetAdditionalParametersOnTemplate(request);
        TrySetTemplateContextOnTemplate(request.Template, request.Model, request.Context);
        TrySetTemplateEngineOnTemplate(request.Template, engine);
    }

    private void TrySetAdditionalParametersOnTemplate(IRenderTemplateRequest request)
    {
        var templateType = request.Template.GetType();
        if (Array.Exists(templateType.GetInterfaces(), t => t.FullName?.StartsWith("TemplateFramework.Abstractions.IModelContainer", StringComparison.InvariantCulture) == true))
        {
            var modelProperty = templateType.GetProperty(nameof(IModelContainer<object?>.Model))!;
            modelProperty.SetValue(request.Template, ConvertType(request.Model, modelProperty.PropertyType));
        }

        if (request.Template is not IParameterizedTemplate parameterizedTemplate)
        {
            return;
        }

        var session = request.AdditionalParameters.ToKeyValuePairs();
        var parameters = parameterizedTemplate.GetParameters();
        foreach (var item in session.Where(x => x.Key != Constants.ModelKey))
        {
            var parameter = Array.Find(parameters, p => p.Name == item.Key);
            if (parameter is null)
            {
                throw new NotSupportedException($"Unsupported template parameter: {item.Key}");
            }

            parameterizedTemplate.SetParameter(item.Key, ConvertType(item.Value, parameter.Type));
        }
    }

    private static void TrySetTemplateContextOnTemplate(object template, object? model, ITemplateContext? context)
    {
        if (context is null)
        {
            context = new TemplateContext(template, model);
        }

        if (template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }

        templateContextContainer.Context = context;
    }

    private static void TrySetTemplateEngineOnTemplate(object template, ITemplateEngine engine)
    {
        if (template is not ITemplateEngineContainer templateEngineContainer)
        {
            return;
        }

        templateEngineContainer.Engine = engine;
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
