namespace TemplateFramework.Core.MultipleContentBuilderTemplateCreators;

//TODO: Review if we want this. This is purely a backwards compatibility thing.
public class TypedMultipleCreator : TypedMultipleCreator<StringBuilder>
{
}

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
