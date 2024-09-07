namespace TemplateFramework.Core.Abstractions;

public interface ITemplateRenderer
{
    bool Supports(IGenerationEnvironment generationEnvironment);
    Task<Result> Render(ITemplateEngineContext context, CancellationToken cancellationToken);
}
