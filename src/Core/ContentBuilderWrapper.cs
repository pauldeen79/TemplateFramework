namespace TemplateFramework.Core;

internal sealed class ContentBuilderWrapper : IContentBuilder
{
    private readonly object _instance;

    public ContentBuilderWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        _instance = instance;
    }

    public string? Filename
    {
        get => (string?)_instance.GetType().GetProperty(nameof(Filename))?.GetValue(_instance);
        set => _instance.GetType().GetProperty(nameof(Filename))?.SetValue(_instance, value);
    }

    public bool SkipWhenFileExists
    {
        get => NullGuard((bool?)_instance.GetType().GetProperty(nameof(SkipWhenFileExists))?.GetValue(_instance), nameof(SkipWhenFileExists));
        set => _instance.GetType().GetProperty(nameof(SkipWhenFileExists))?.SetValue(_instance, value);
    }

    public StringBuilder Builder
    {
        get => NullGuard((StringBuilder?)_instance.GetType().GetProperty(nameof(Builder))?.GetValue(_instance), nameof(Builder));
        set => _instance.GetType().GetProperty(nameof(Builder))?.SetValue(_instance, value);
    }

    public IContent Build()
        => new ContentWrapper(NullGuard(_instance.GetType().GetMethod(nameof(Build))?.Invoke(_instance, Array.Empty<object?>()), nameof(Build)));

    private T NullGuard<T>(T? value, string name)
        => value is null
            ? throw new InvalidOperationException($"{name} of content builder {_instance.GetType().FullName} was null")
            : value;

    private T NullGuard<T>(T? value, string name) where T : struct
        => value is null
            ? throw new InvalidOperationException($"{name} of content builder {_instance.GetType().FullName} was null")
            : value.Value;
}
