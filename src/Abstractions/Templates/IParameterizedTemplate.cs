namespace TemplateFramework.Abstractions.Templates;

public interface IParameterizedTemplate
{
    Result SetParameter(string name, object? value);
    Result<ITemplateParameter[]> GetParameters();
}
