namespace TemplateFramework.Abstractions;

public interface ITemplateParameterExtractor
{
    ITemplateParameter[] Extract(object templateInstance);
}
