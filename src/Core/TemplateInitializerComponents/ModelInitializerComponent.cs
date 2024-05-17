﻿namespace TemplateFramework.Core.TemplateInitializerComponents;

public class ModelInitializerComponent : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ModelInitializerComponent(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);
        Guard.IsNotNull(context.Template);

        var templateType = context.Template.GetType();
        
        if (Array.Exists(templateType.GetInterfaces(), t => t.FullName?.StartsWith("TemplateFramework.Abstractions.IModelContainer", StringComparison.InvariantCulture) == true))
        {
            var modelProperty = templateType.GetProperty(nameof(IModelContainer<object?>.Model))!;
            modelProperty.SetValue(context.Template, _converter.Convert(context.Model, modelProperty.PropertyType, context));
        }

        return Task.CompletedTask;
    }
}
