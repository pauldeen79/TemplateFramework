namespace TemplateFramework.Core;

internal sealed class MultipleContentBuilderTemplateWrapper : IMultipleContentBuilderTemplate
{
    private readonly object _instance;

    public MultipleContentBuilderTemplateWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        _instance = instance;
    }

    public void Render(IMultipleContentBuilder builder)
    {
        Guard.IsNotNull(builder);

        NullGuard
        (
            Array.Find(_instance.GetType().GetMethods(), m => m.Name == nameof(Render) && m.GetParameters().Length == 1 && Array.Exists(m.GetParameters(), p => p.ParameterType.Name == nameof(IMultipleContentBuilder))),
            nameof(Render)
        ).Invoke(_instance, new object?[] { new MultipleContentBuilderWrapper(builder) });
    }

    private T NullGuard<T>(T? value, string name)
        => value is null
            ? throw new InvalidOperationException($"{name} of multiple content builder template {_instance.GetType().FullName} was null")
            : value;
}
