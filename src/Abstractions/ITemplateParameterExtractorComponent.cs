namespace TemplateFramework.Abstractions;

public interface ITemplateParameterExtractorComponent : ITemplateParameterExtractor
{
    bool Supports(object templateInstance);
}
