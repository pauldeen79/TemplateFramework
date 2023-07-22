namespace TemplateFramework.Abstractions.Templates;

public interface ITemplate
{
    void Render(StringBuilder builder);
}
