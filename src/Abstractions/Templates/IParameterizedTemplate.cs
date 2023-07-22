namespace TemplateFramework.Abstractions.Templates;

public interface IParameterizedTemplate
{
    void SetParameter(string name, object? value);
    ITemplateParameter[] GetParameters();
}
