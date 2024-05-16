namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class TypedTextTransformTemplateRenderer : IStringBuilderTemplateRenderer
{
    public Task<bool> TryRender(object instance, StringBuilder builder, CancellationToken cancellationToken)
    {
        if (instance is ITextTransformTemplate textTransformTemplate)
        {
            Guard.IsNotNull(builder);

            var output = textTransformTemplate.TransformText();
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
