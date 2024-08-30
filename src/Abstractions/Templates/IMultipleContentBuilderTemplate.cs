namespace TemplateFramework.Abstractions.Templates;

public interface IMultipleContentBuilderTemplate : IMultipleContentBuilderTemplate<StringBuilder>
{
}

public interface IMultipleContentBuilderTemplate<T> : IBuilderTemplate<IMultipleContentBuilder<T>> where T : class
{
}
