namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ParameterInitializerComponent : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ParameterInitializerComponent(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public void Initialize(ITemplateEngineContext context)
    {
        Guard.IsNotNull(context);

        if (context.Template is not IParameterizedTemplate parameterizedTemplate)
        {
            return;
        }

        var session = context.AdditionalParameters.ToKeyValuePairs();
        var parameters = parameterizedTemplate.GetParameters();
        foreach (var item in session.Where(x => x.Key != Constants.ModelKey))
        {
            var parameter = Array.Find(parameters, p => p.Name == item.Key);
            if (parameter is null)
            {
                throw new NotSupportedException($"Unsupported template parameter: {item.Key}");
            }

            parameterizedTemplate.SetParameter(item.Key, _converter.Convert(item.Value, parameter.Type));
        }
    }
}
