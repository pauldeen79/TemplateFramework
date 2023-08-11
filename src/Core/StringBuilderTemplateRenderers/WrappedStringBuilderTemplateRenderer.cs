namespace TemplateFramework.Core.StringBuilderTemplateRenderers;

public class WrappedStringBuilderTemplateRenderer : IStringBuilderTemplateRenderer
{
    public bool TryRender(object instance, StringBuilder builder)
    {
        if (instance is null)
        {
            return false;
        }

        var renderMethod = Array.Find(instance.GetType().GetMethods(),
            m => m.Name == nameof(IStringBuilderTemplate.Render)
            && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(StringBuilder));

        if (renderMethod is null)
        {
            return false;
        }

        renderMethod.Invoke(instance, new object?[] { builder });

        return true;
    }
}
