namespace TemplateFramework.Core.Abstractions;

public interface ITemplateEngineContext : IRenderTemplateRequest
{
    ITemplateEngine Engine { get; }
    ITemplateComponentRegistry ComponentRegistry { get; }
    object? Template { get; }
}
