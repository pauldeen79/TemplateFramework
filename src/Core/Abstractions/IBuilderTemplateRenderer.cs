namespace TemplateFramework.Core.Abstractions;

public interface IBuilderTemplateRenderer<TBuilder>
{
    Task<bool> TryRender(object instance, TBuilder builder, CancellationToken cancellationToken);
}
