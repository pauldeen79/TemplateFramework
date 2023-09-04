namespace TemplateFramework.Core.TemplateIdentifiers;

public class TemplateInstanceIdentifier : ITemplateIdentifier
{
    public TemplateInstanceIdentifier(object instance)
    {
        Guard.IsNotNull(instance);

        Instance = instance;
    }

    public object Instance { get; }
}
