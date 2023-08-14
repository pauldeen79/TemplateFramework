namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ParameterInitializer : ITemplateInitializerComponent
{
    private readonly IEnumerable<ITemplateParameterConverter> _converters;

    public ParameterInitializer(IEnumerable<ITemplateParameterConverter> converters)
    {
        Guard.IsNotNull(converters);

        _converters = converters;
    }

    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

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
