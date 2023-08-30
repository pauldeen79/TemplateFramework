namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.TemplateIdentifiers;

public sealed class TemplateByNameIdentifier : ITemplateIdentifier
{
    public TemplateByNameIdentifier(string name)
    {
        Guard.IsNotNullOrEmpty(name);

        Name = name;
    }

    public string Name { get; }
}
