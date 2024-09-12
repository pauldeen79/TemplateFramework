namespace TemplateFramework.Core.Abstractions;

public interface ITemplateInitializer
{
    Task<Result> Initialize(ITemplateEngineContext context, CancellationToken cancellationToken);
}
