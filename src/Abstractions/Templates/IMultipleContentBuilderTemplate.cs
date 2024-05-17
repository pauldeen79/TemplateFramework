namespace TemplateFramework.Abstractions.Templates;

public interface IMultipleContentBuilderTemplate
{
    Task Render(IMultipleContentBuilder builder, CancellationToken cancellationToken);
}
