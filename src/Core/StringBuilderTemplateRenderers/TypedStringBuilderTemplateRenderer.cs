namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class TypedStringBuilderTemplateRenderer : IStringBuilderTemplateRenderer
{
    public Task<bool> TryRender(object instance, StringBuilder builder)
    {
        if (instance is IStringBuilderTemplate typedTemplate)
        {
            typedTemplate.Render(builder);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
