namespace TemplateFramework.Core.Abstractions;

public interface ITemplateEngineContext : IRenderTemplateRequest
{
    ITemplateEngine Engine { get; }
    object? Template { get; }
}
