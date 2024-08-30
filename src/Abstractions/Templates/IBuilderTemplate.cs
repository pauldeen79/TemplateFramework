namespace TemplateFramework.Abstractions.Templates;

public interface IBuilderTemplate<in T>
{
    Task Render(T builder, CancellationToken cancellationToken);
}
