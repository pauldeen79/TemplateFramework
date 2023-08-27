namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

internal sealed class TemplateParameter : ITemplateParameter
{
    public TemplateParameter(string name, Type type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public Type Type { get; }
}
