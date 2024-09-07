namespace TemplateFramework.Core.Abstractions;

public interface IBuilderTemplateRenderer<TBuilder>
{
    //TODO: Convert bool to Result<bool>, so you can also return error messages etc.
    Task<bool> TryRender(object instance, TBuilder builder, CancellationToken cancellationToken);
}
