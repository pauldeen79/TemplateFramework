namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ParameterInitializerComponent : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ParameterInitializerComponent(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public int Order => 5;

    public async Task<Result> InitializeAsync(ITemplateEngineContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.Template is IParameterizedTemplate parameterizedTemplate)
        {
            return await SetTyped(context, parameterizedTemplate, token).ConfigureAwait(false);
        }

        return await SetProperties(context, token).ConfigureAwait(false);
    }

    private async Task<Result> SetTyped(ITemplateEngineContext context, IParameterizedTemplate parameterizedTemplate, CancellationToken token)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var result = await parameterizedTemplate.GetParametersAsync(token).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return result;
        }

        foreach (var item in session.Where(x => x.Key != Constants.ModelKey))
        {
            var parameter = Array.Find(result.Value!, p => p.Name == item.Key);
            if (parameter is null)
            {
                continue;
            }

            var conversionResult = await _converter.Convert(item.Value, parameter.Type, context)
                .OnSuccessAsync(value => parameterizedTemplate.SetParameterAsync(item.Key, value, token))
                .ConfigureAwait(false);

            if (!conversionResult.IsSuccessful())
            {
                return conversionResult;
            }
        }

        return Result.Success();
    }

    private async Task<Result> SetProperties(ITemplateEngineContext context, CancellationToken token)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var result = await context.Engine.GetParametersAsync(context.Template!, token).ConfigureAwait(false);
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

            var conversionResult = _converter.Convert(item.Value, prop.PropertyType, context);
            if (!conversionResult.IsSuccessful())
            {
                return conversionResult;
            }

            prop.SetValue(context.Template, conversionResult.Value);
        }

        return Result.Success();
    }
}
