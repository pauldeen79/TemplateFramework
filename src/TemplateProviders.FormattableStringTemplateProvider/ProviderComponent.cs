namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public class ProviderComponent : ITemplateProviderComponent, ISessionAwareComponent
{
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

    public ProviderComponent(IFormattableStringParser formattableStringParser, ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(formattableStringParser);
        Guard.IsNotNull(componentRegistrationContext);

        _formattableStringParser = formattableStringParser;
        _componentRegistrationContext = componentRegistrationContext;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is FormattableStringTemplateIdentifier;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        if (identifier is FormattableStringTemplateIdentifier createFormattableStringTemplateRequest)
        {
            return new FormattableStringTemplate(createFormattableStringTemplateRequest, _formattableStringParser, _componentRegistrationContext);
        }
        else
        {
            throw new NotSupportedException($"Identifier of type {identifier.GetType().FullName} is not supported");
        }
    }

    public void StartSession() => _componentRegistrationContext.Processors.Clear();
}
