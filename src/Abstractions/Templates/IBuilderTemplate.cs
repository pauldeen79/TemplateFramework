namespace TemplateFramework.Abstractions.Templates;

public interface IBuilderTemplate<in T>
{
    Task<Result> Render(T builder, CancellationToken cancellationToken);
}
