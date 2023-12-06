namespace TemplateFramework.Core;

public class ValueConverter : IValueConverter
{
    private readonly IEnumerable<ITemplateParameterConverter> _converters;

    public ValueConverter(IEnumerable<ITemplateParameterConverter> converters)
    {
        Guard.IsNotNull(converters);

        _converters = converters;
    }

    public object? Convert(object? value, Type type, ITemplateEngineContext context)
    {
        Guard.IsNotNull(type);
        Guard.IsNotNull(context);

        foreach (var converter in _converters)
        {
            if (converter.TryConvert(value, type, context, out var convertedValue))
            {
                return convertedValue;
            }
        }

        return value;
    }
}
