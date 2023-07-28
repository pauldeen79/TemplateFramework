namespace TemplateFramework.Core;

public sealed class TemplateParameter : ITemplateParameter
{
    public TemplateParameter(string name, Type type)
    {
        Guard.IsNotNull(name);
        Guard.IsNotNull(type);
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public Type Type { get; }
}
