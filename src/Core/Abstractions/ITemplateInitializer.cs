namespace TemplateFramework.Core.Abstractions;

public interface ITemplateInitializer
{
    Task Initialize(ITemplateEngineContext context, CancellationToken cancellationToken);
}
