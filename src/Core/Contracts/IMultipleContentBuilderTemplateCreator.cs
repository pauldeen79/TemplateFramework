namespace TemplateFramework.Core.Contracts;

public interface IMultipleContentBuilderTemplateCreator
{
    IMultipleContentBuilderTemplate? TryCreate(object instance);
}
