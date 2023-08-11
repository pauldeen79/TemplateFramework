namespace TemplateFramework.Core.Contracts;

public interface IStringBuilderTemplateRenderer
{
    bool TryRender(object instance, StringBuilder builder);
}
