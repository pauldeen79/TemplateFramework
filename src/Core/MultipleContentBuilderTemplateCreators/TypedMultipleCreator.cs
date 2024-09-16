namespace TemplateFramework.Core.MultipleContentBuilderTemplateCreators;

public class TypedMultipleCreator<T> : IMultipleContentBuilderTemplateCreator<T> where T : class
{
    public IMultipleContentBuilderTemplate<T>? TryCreate(object instance)
    {
        if (instance is IMultipleContentBuilderTemplate<T> multipleContentBuilderTemplate)
        {
            return multipleContentBuilderTemplate;
        }

        return null;
    }
}
