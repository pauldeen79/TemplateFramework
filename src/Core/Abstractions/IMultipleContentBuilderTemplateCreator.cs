namespace TemplateFramework.Core.Abstractions;

public interface IMultipleContentBuilderTemplateCreator
{
    IMultipleContentBuilderTemplate? TryCreate(object instance);
}
