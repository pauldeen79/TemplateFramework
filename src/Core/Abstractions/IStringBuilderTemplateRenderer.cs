namespace TemplateFramework.Core.Abstractions;

public interface IStringBuilderTemplateRenderer
{
    Task<bool> TryRender(object instance, StringBuilder builder, CancellationToken cancellationToken);
}
