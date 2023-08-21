﻿namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public class TemplateWrapper : ITemplateContextContainer, IParameterizedTemplate, IMultipleContentBuilderTemplate, IModelContainer<object>
{
    private readonly object _instance;

    public TemplateWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        _instance = instance;
    }

    public ITemplateContext Context { get; set; } = default!;
    public object? Model { get; set; }

    public ITemplateParameter[] GetParameters()
    {
        var method = _instance.GetType().GetMethod(nameof(GetParameters));
        if (method is null)
        {
            return Array.Empty<ITemplateParameter>();
        }

        var methodResult = method.Invoke(_instance, Array.Empty<object>());
        if (methodResult is not IEnumerable enumerable)
        {
            throw new NotSupportedException("GetParameters method did not return an Enumerable");
        }

        return enumerable
            .OfType<object>()
            .Select(x => new TemplateParameterWrapper(x))
            .ToArray();
    }

    public void Render(IMultipleContentBuilder builder)
    {
        Guard.IsNotNull(builder);

        var type = _instance.GetType();
        InitializeModel(type);

        var transformText = type.GetMethod(nameof(ITextTransformTemplate.TransformText));
        if (transformText is not null)
        {
            var result = transformText.Invoke(_instance, Array.Empty<object?>())?.ToString();
            // Render using a stringbuilder, then add it to multiple contents
            builder.AddContent(Context?.DefaultFilename ?? string.Empty, false, new StringBuilder(result ?? string.Empty));
            return;
        }

        var renderMethod = Array.Find(type.GetMethods(), m => m.Name == nameof(Render) && m.GetParameters().Length == 1);
        if (renderMethod is null)
        {
            // There is no Render or TransformText method, so call ToString
            var result = GetToStringMethod(type).Invoke(_instance, Array.Empty<object?>());
            // Render using a stringbuilder, then add it to multiple contents
            builder.AddContent(Context?.DefaultFilename ?? string.Empty, false, new StringBuilder(result?.ToString() ?? string.Empty));
            return;
        }

        if (renderMethod.GetParameters()[0].ParameterType.Name == nameof(IMultipleContentBuilder))
        {
            var result = new WrappedMultipleCreator().TryCreate(_instance);
            
            Guard.IsNotNull(result);
            
            result.Render(builder);
        }
        else if (renderMethod.GetParameters()[0].ParameterType == typeof(StringBuilder))
        {
            // Render using a stringbuilder, then add it to multiple contents
            var stringBuilder = new StringBuilder();
            renderMethod.Invoke(_instance, new object?[] { stringBuilder });
            builder.AddContent(Context?.DefaultFilename ?? string.Empty, false, stringBuilder);
        }
        else
        {
            // There is no compatible Render method, so call ToString
            var result = GetToStringMethod(type).Invoke(_instance, Array.Empty<object?>());
            // Render using a stringbuilder, then add it to multiple contents
            builder.AddContent(Context?.DefaultFilename ?? string.Empty, false, new StringBuilder(result?.ToString() ?? string.Empty));
        }
    }

    public void SetParameter(string name, object? value)
    {
        var method = _instance.GetType().GetMethod(nameof(SetParameter));
        if (method is null)
        {
            return;
        }

        method.Invoke(_instance, new object?[] { name, value });
    }

    private void InitializeModel(Type type)
    {
        if (Model is null)
        {
            return;
        }

        var modelProperty = type.GetProperty(nameof(IModelContainer<object>.Model));
        if (modelProperty is not null)
        {
            modelProperty.SetValue(_instance, Model);
        }
    }

    private static MethodInfo GetToStringMethod(Type type)
        => type.GetMethods().First(x => x.Name == nameof(ToString) && x.GetParameters().Length == 0);
}
