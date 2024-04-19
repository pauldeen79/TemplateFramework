namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class TypedTextTransformTemplateRenderer : IStringBuilderTemplateRenderer
{
    public bool TryRender(object instance, StringBuilder builder)
    {
        if (instance is ITextTransformTemplate textTransformTemplate)
        {
            Guard.IsNotNull(builder);

            var output = textTransformTemplate.TransformText();
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }

            return true;
        }

        return false;
    }
}
