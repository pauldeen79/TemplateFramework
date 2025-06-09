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

    public async Task<Result> Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        if (context.Template is IParameterizedTemplate parameterizedTemplate)
        {
            return await SetTyped(context, parameterizedTemplate, cancellationToken).ConfigureAwait(false);
        }

        return await TrySetProperties(context, cancellationToken).ConfigureAwait(false);
    }

    private async Task<Result> SetTyped(ITemplateEngineContext context, IParameterizedTemplate parameterizedTemplate, CancellationToken cancellationToken)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var result = await parameterizedTemplate.GetParametersAsync(cancellationToken).ConfigureAwait(false);
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

            var setParametersResult = await parameterizedTemplate.SetParameterAsync(item.Key, _converter.Convert(item.Value, parameter.Type, context), cancellationToken).ConfigureAwait(false);
            if (!setParametersResult.IsSuccessful())
            {
                return setParametersResult;
            }
        }

        return Result.Success();
    }

    private async Task<Result> TrySetProperties(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var result = await context.Engine.GetParametersAsync(context.Template!, cancellationToken).ConfigureAwait(false);
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

        return Result.Success();
    }
}
