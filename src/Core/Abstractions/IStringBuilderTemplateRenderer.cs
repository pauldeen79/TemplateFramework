namespace TemplateFramework.Core.Abstractions;

public interface IStringBuilderTemplateRenderer
{
    bool TryRender(object instance, StringBuilder builder);
}
