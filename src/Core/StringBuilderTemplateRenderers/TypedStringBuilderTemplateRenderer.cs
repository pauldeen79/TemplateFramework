namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class TypedStringBuilderTemplateRenderer : IStringBuilderTemplateRenderer
{
    public async Task<bool> TryRender(object instance, StringBuilder builder, CancellationToken cancellationToken)
    {
        if (instance is IStringBuilderTemplate typedTemplate)
        {
            await typedTemplate.Render(builder, cancellationToken).ConfigureAwait(false);
            return true;
        }

        return false;
    }
}
