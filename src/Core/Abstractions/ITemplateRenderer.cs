namespace TemplateFramework.Core.Abstractions;

public interface ITemplateRenderer
{
    bool Supports(IGenerationEnvironment generationEnvironment);
    Task<Result> RenderAsync(ITemplateEngineContext context, CancellationToken cancellationToken);
}
