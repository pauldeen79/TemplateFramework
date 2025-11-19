namespace TemplateFramework.Abstractions.Templates;

public interface IBuilderTemplate<in T>
{
    Task<Result> RenderAsync(T builder, CancellationToken token);
}
