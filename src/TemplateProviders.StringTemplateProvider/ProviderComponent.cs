namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ProviderComponent : ITemplateProviderComponent, ISessionAwareComponent
{
    private readonly IExpressionStringParser _expressionStringParser;
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

    public ProviderComponent(
        IExpressionStringParser expressionStringParser,
        IFormattableStringParser formattableStringParser,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(expressionStringParser);
        Guard.IsNotNull(formattableStringParser);
        Guard.IsNotNull(componentRegistrationContext);

        _expressionStringParser = expressionStringParser;
        _formattableStringParser = formattableStringParser;
        _componentRegistrationContext = componentRegistrationContext;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is ExpressionStringTemplateIdentifier or FormattableStringTemplateIdentifier;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        if (identifier is ExpressionStringTemplateIdentifier expressionStringTemplateIdentifier)
        {
            return new ExpressionStringTemplate(expressionStringTemplateIdentifier, _expressionStringParser, _formattableStringParser, _componentRegistrationContext);
        }
        else if (identifier is FormattableStringTemplateIdentifier formattableStringTemplateIdentifier)
        {
            return new FormattableStringTemplate(formattableStringTemplateIdentifier, _formattableStringParser, _componentRegistrationContext);
        }
        else
        {
            throw new NotSupportedException($"Identifier of type {identifier.GetType().FullName} is not supported");
        }
    }

    public void StartSession()
    {
        _componentRegistrationContext.PlaceholderProcessors.Clear();
        _componentRegistrationContext.FunctionResultParsers.Clear();
    }
}
