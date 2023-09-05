namespace TemplateFramework.Core.Abstractions;

public interface ITemplateEngineContext : IRenderTemplateRequest
{
    ITemplateEngine Engine { get; }
    ITemplateProvider Provider { get; }
    object? Template { get; }
}
