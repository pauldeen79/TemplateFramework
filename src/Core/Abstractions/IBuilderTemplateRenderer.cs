namespace TemplateFramework.Core.Abstractions;

public interface IBuilderTemplateRenderer<TBuilder>
{
    //TODO: Convert bool to Result, so you can also return error messages etc. And return Success() when supported or NotSupported() when next renderer should be used
    Task<bool> TryRender(object instance, TBuilder builder, CancellationToken cancellationToken);
}
