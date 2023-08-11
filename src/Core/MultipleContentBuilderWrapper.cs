namespace TemplateFramework.Core;

internal sealed class MultipleContentBuilderWrapper : IMultipleContentBuilder
{
    private readonly object _instance;

    public MultipleContentBuilderWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        _instance = instance;
    }

    public IEnumerable<IContentBuilder> Contents => NullGuard((IEnumerable<object>?)NullGuard(_instance.GetType().GetProperty(nameof(Contents)), nameof(Contents)).GetValue(_instance), nameof(Contents)).Select(x => new ContentBuilderWrapper(x));

    public IContentBuilder AddContent(string filename, bool skipWhenFileExists, StringBuilder? builder)
        => new ContentBuilderWrapper(NullGuard(NullGuard(_instance.GetType().GetMethod(nameof(AddContent)), nameof(AddContent)).Invoke(_instance, new object?[] { filename, skipWhenFileExists, builder }), nameof(AddContent)));

    public IMultipleContent Build()
        => new MultipleContentWrapper(NullGuard(NullGuard(_instance.GetType().GetMethod(nameof(Build)), nameof(Build)).Invoke(_instance, Array.Empty<object?>()), nameof(Build)));

    private T NullGuard<T>(T? value, string name)
        => value is null
            ? throw new InvalidOperationException($"{name} of multiple content builder {_instance.GetType().FullName} was null")
            : value;
}
