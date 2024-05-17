namespace TemplateFramework.Abstractions.Templates;

public interface IStringBuilderTemplate
{
    Task Render(StringBuilder builder, CancellationToken cancellationToken);
}
