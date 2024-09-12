namespace TemplateFramework.Abstractions;

public interface ITemplateParameterExtractor
{
    Result<ITemplateParameter[]> Extract(object templateInstance);
}
