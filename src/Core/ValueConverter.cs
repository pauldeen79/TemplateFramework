namespace TemplateFramework.Core;

public class ValueConverter : IValueConverter
{
    private readonly IEnumerable<ITemplateParameterConverter> _converters;

    public ValueConverter(IEnumerable<ITemplateParameterConverter> converters)
    {
        Guard.IsNotNull(converters);

        _converters = converters;
    }

    public Result<object?> Convert(object? value, Type type, ITemplateEngineContext context)
    {
        Guard.IsNotNull(type);
        Guard.IsNotNull(context);

        foreach (var converter in _converters)
        {
            var result = converter.Convert(value, type, context);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.Success(value);
    }
}
