namespace TemplateFramework.Core.Abstractions;

public interface ITemplateInitializer
{
    Task<Result> InitializeAsync(ITemplateEngineContext context, CancellationToken cancellationToken);
}
