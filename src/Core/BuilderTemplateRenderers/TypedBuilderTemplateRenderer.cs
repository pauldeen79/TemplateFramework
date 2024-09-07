namespace TemplateFramework.Core.BuilderTemplateRenderers;

public class TypedBuilderTemplateRenderer<TBuilder> : IBuilderTemplateRenderer<TBuilder>
{
    public async Task<bool> TryRender(object instance, TBuilder builder, CancellationToken cancellationToken)
    {
        if (instance is IBuilderTemplate<TBuilder> typedTemplate)
        {
            await typedTemplate.Render(builder, cancellationToken).ConfigureAwait(false);
            return true;
        }

        return false;
    }
}
