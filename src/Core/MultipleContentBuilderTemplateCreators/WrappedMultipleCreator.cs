namespace TemplateFramework.Core.MultipleContentBuilderTemplateCreators;

public class WrappedMultipleCreator : IMultipleContentBuilderTemplateCreator
{
    public IMultipleContentBuilderTemplate? TryCreate(object instance)
    {
        if (instance is null)
        {
            return null;
        }

        if (Array.Exists(instance.GetType().GetInterfaces(), i => i.FullName == typeof(IMultipleContentBuilderTemplate).FullName))
        {
            return new MultipleContentBuilderTemplateWrapper(instance);
        }

        return null;
    }
}
