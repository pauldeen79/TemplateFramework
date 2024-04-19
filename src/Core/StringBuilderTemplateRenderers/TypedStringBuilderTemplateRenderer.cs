namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class TypedStringBuilderTemplateRenderer : IStringBuilderTemplateRenderer
{
    public bool TryRender(object instance, StringBuilder builder)
    {
        if (instance is IStringBuilderTemplate typedTemplate)
        {
            typedTemplate.Render(builder);
            return true;
        }

        return false;
    }
}
