namespace TemplateFramework.Core;

internal sealed class MultipleContentWrapper : IMultipleContent
{
    public MultipleContentWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        Contents = NullGuard((IEnumerable<object?>?)instance.GetType().GetProperty(nameof(Contents))?.GetValue(instance), instance, nameof(Contents))
            .Select(x => new ContentWrapper(x!))
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyCollection<IContent> Contents { get; }

    private T NullGuard<T>(T? value, object instance, string name)
        => value is null
        ? throw new InvalidOperationException($"{name} of multiple content {instance.GetType().FullName} was null")
        : value;
}
