namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class WrappedTextTransformTemplateRenderer : IStringBuilderTemplateRenderer
{
    public bool TryRender(object instance, StringBuilder builder)
    {
        if (instance is null)
        {
            return false;
        }

        var renderMethod = Array.Find(instance.GetType().GetMethods(),
            m => m.Name == nameof(ITextTransformTemplate.TransformText)
            && m.ReturnType == typeof(string)
            && m.GetParameters().Length == 0);

        if (renderMethod is null)
        {
            return false;
        }

        var result = (string?)renderMethod.Invoke(instance, Array.Empty<object?>());

        if (!string.IsNullOrEmpty(result))
        {
            Guard.IsNotNull(builder);
            builder.Append(result);
        }

        return true;
    }
}
