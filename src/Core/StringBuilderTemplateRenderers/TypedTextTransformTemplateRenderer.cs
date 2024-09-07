namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class TypedTextTransformTemplateRenderer : IBuilderTemplateRenderer<StringBuilder>
{
    public async Task<bool> TryRender(object instance, StringBuilder builder, CancellationToken cancellationToken)
    {
        if (instance is ITextTransformTemplate textTransformTemplate)
        {
            Guard.IsNotNull(builder);

            var output = await textTransformTemplate.TransformText(cancellationToken).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }

            return true;
        }

        return false;
    }
}
