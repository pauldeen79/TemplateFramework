namespace TemplateFramework.Core.Abstractions;

public interface IBuilderTemplateRenderer<TBuilder>
{
    Task<Result> RenderAsync(object instance, TBuilder builder, CancellationToken token);
}
