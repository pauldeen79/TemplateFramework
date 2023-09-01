namespace TemplateFramework.Core.TemplateProviderComponents;

public class TemplateInstanceIdentifierComponent : ITemplateProviderComponent
{
    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        Guard.IsOfType<TemplateInstanceIdentifier>(identifier);

        return ((TemplateInstanceIdentifier)identifier).Instance;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is TemplateInstanceIdentifier;
}
