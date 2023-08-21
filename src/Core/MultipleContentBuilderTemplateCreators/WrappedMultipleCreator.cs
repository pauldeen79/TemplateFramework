namespace TemplateFramework.Core.MultipleContentBuilderTemplateCreators;

public class WrappedMultipleCreator : IMultipleContentBuilderTemplateCreator
{
    public IMultipleContentBuilderTemplate? TryCreate(object instance)
    {
        if (instance is null)
        {
            return null;
        }

        if (Array.Exists(instance.GetType().GetMethods(), m =>
            m.Name == nameof(IMultipleContentBuilderTemplate.Render)
            && Array.TrueForAll(m.GetParameters(), p => p.ParameterType.Name == nameof(IMultipleContentBuilder))))
        {
            return new MultipleContentBuilderTemplateWrapper(instance);
        }

        return null;
    }
}
