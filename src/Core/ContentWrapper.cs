namespace TemplateFramework.Core;

internal sealed class ContentWrapper : IContent
{
    private readonly object _instance;

    public ContentWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        _instance = instance;
    }

    public string Filename => NullGuard((string?)_instance.GetType().GetProperty(nameof(Filename))?.GetValue(_instance), nameof(Filename));

    public bool SkipWhenFileExists => NullGuard((bool?)_instance.GetType().GetProperty(nameof(SkipWhenFileExists))?.GetValue(_instance), nameof(SkipWhenFileExists));

    public string Contents => NullGuard((string?)_instance.GetType().GetProperty(nameof(Contents))?.GetValue(_instance), nameof(Contents));

    private T NullGuard<T>(T? value, string name)
        => value is null
            ? throw new InvalidOperationException($"{name} of content {_instance.GetType().FullName} was null")
            : value;

    private T NullGuard<T>(T? value, string name) where T : struct
        => value is null
            ? throw new InvalidOperationException($"{name} of content {_instance.GetType().FullName} was null")
            : value.Value;
}
