namespace TemplateFramework.Core.Abstractions;

public interface IBuilderTemplateRenderer<TBuilder>
{
    Task<Result> TryRender(object instance, TBuilder builder, CancellationToken cancellationToken);
}
