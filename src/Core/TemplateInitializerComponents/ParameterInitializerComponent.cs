namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ParameterInitializerComponent : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ParameterInitializerComponent(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public async Task<Result> Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.Template is IParameterizedTemplate parameterizedTemplate)
        {
            return SetTyped(context, parameterizedTemplate);
        }

        return await TrySetProperties(context).ConfigureAwait(false);
    }

    private Result SetTyped(ITemplateEngineContext context, IParameterizedTemplate parameterizedTemplate)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var result = parameterizedTemplate.GetParameters();
        if (!result.IsSuccessful())
        {
            return result;
        }

        foreach (var item in session.Where(x => x.Key != Constants.ModelKey))
        {
            var parameter = Array.Find(result.GetValueOrThrow(), p => p.Name == item.Key);
            if (parameter is null)
            {
                continue;
            }

            var setParametersResult = parameterizedTemplate.SetParameter(item.Key, _converter.Convert(item.Value, parameter.Type, context));
            if (!setParametersResult.IsSuccessful())
            {
                return setParametersResult;
            }
        }

        return Result.Success();
    }

    private async Task<Result> TrySetProperties(ITemplateEngineContext context)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var result = await context.Engine.GetParameters(context.Template!).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return result;
        }

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

        return result;
    }
}
