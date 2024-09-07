namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ParameterInitializerComponent : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ParameterInitializerComponent(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public async Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        if (context.Template is IParameterizedTemplate parameterizedTemplate)
        {
            SetTyped(context, parameterizedTemplate);
        }
        else if (context.Template is not null)
        {
            await TrySetProperties(context).ConfigureAwait(false);
        }
    }

    private void SetTyped(ITemplateEngineContext context, IParameterizedTemplate parameterizedTemplate)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var parameters = parameterizedTemplate.GetParameters();

        foreach (var item in session.Where(x => x.Key != Constants.ModelKey))
        {
            var parameter = Array.Find(parameters, p => p.Name == item.Key);
            if (parameter is null)
            {
                continue;
            }

            parameterizedTemplate.SetParameter(item.Key, _converter.Convert(item.Value, parameter.Type, context));
        }
    }

    private async Task TrySetProperties(ITemplateEngineContext context)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var result = await context.Engine.GetParameters(context.Template!).ConfigureAwait(false);
        var parameters = result.GetValueOrThrow();
        var templateType = context.Template!.GetType();

        foreach (var item in session.Where(x => x.Key != Constants.ModelKey))
        {
            var parameter = Array.Find(parameters, p => p.Name == item.Key);
            if (parameter is null)
            {
                continue;
            }

            var prop = templateType.GetProperty(item.Key);
            if (prop is null)
            {
                continue;
            }

            prop.SetValue(context.Template, _converter.Convert(item.Value, prop.PropertyType, context));
        }
    }
}
