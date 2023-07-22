namespace TemplateFramework.Abstractions.Templates;

public interface ITemplateParameter
{
    string Name { get; }
    Type Type { get; }
}
