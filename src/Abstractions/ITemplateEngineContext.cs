namespace TemplateFramework.Abstractions;

public interface ITemplateEngineContext : IRenderTemplateRequest
{
    ITemplateEngine Engine { get; }
    ITemplateComponentRegistry ComponentRegistry { get; }
    IDictionary<string, object?> ParametersDictionary { get; }
    object? Template { get; }
}
