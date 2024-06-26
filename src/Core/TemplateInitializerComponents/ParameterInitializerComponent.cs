﻿namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ParameterInitializerComponent : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ParameterInitializerComponent(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        if (context.Template is IParameterizedTemplate parameterizedTemplate)
        {
            SetTyped(context, parameterizedTemplate);
        }
        else if (context.Template is not null)
        {
            TrySetProperties(context);
        }

        return Task.CompletedTask;
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

    private void TrySetProperties(ITemplateEngineContext context)
    {
        var session = context.AdditionalParameters.ToKeyValuePairs();
        var parameters = context.Engine.GetParameters(context.Template!);
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
