namespace TemplateFramework.Core.Abstractions;

public interface IMultipleContentBuilderTemplateCreator<T> where T : class
{
    IMultipleContentBuilderTemplate<T>? TryCreate(object instance);
}
