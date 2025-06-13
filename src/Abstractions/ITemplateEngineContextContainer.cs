namespace TemplateFramework.Abstractions;

public interface ITemplateEngineContextContainer
{
    ITemplateEngineContext Context { get; set; }
}
