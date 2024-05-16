namespace TemplateFramework.Core.Abstractions;

public interface ITemplateRenderer
{
    bool Supports(IGenerationEnvironment generationEnvironment);
    Task Render(ITemplateEngineContext context, CancellationToken cancellationToken);
}
