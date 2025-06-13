namespace TemplateFramework.Core.Abstractions;

public interface IBuilderTemplateRenderer<TBuilder>
{
    Task<Result> TryRenderAsync(object instance, TBuilder builder, CancellationToken cancellationToken);
}
