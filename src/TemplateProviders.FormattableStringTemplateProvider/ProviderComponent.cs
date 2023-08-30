namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public class ProviderComponent : ITemplateProviderComponent
{
    private readonly IFormattableStringParser _formattableStringParser;

    public ProviderComponent(IFormattableStringParser formattableStringParser)
    {
        Guard.IsNotNull(formattableStringParser);

        _formattableStringParser = formattableStringParser;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is FormattableStringTemplateIdentifier;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        Guard.IsOfType<FormattableStringTemplateIdentifier>(identifier);

        var createFormattableStringTemplateRequest = (FormattableStringTemplateIdentifier)identifier;

        return new FormattableStringTemplate(createFormattableStringTemplateRequest, _formattableStringParser);
    }
}
