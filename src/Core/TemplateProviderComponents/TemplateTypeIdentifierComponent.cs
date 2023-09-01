namespace TemplateFramework.Core.TemplateProviderComponents;

public class TemplateTypeIdentifierComponent : ITemplateProviderComponent
{
    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        Guard.IsOfType<TemplateTypeIdentifier>(identifier);

        var typed = (TemplateTypeIdentifier)identifier;
        return typed.TemplateFactory.Create(typed.Type);
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is TemplateTypeIdentifier;
}
