namespace TemplateFramework.Core.MultipleContentBuilderTemplateCreators;

public class TypedMultipleCreator : IMultipleContentBuilderTemplateCreator
{
    public IMultipleContentBuilderTemplate? TryCreate(object instance)
    {
        if (instance is IMultipleContentBuilderTemplate multipleContentBuilderTemplate)
        {
            return multipleContentBuilderTemplate;
        }

        return null;
    }
}
