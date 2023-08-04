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

        _instance.GetType().GetMethod(nameof(Render))?.Invoke(_instance, new object?[] { new MultipleContentBuilderWrapper(builder) });
    }
}
