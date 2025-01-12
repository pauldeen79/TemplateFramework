namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ProviderComponent : ITemplateProviderComponent, ISessionAwareComponent
{
    private readonly IExpressionStringEvaluator _expressionStringEvaluator;
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

    public ProviderComponent(
        IExpressionStringEvaluator expressionStringEvaluator,
        IFormattableStringParser formattableStringParser,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(expressionStringEvaluator);
        Guard.IsNotNull(formattableStringParser);
        Guard.IsNotNull(componentRegistrationContext);

        _expressionStringEvaluator = expressionStringEvaluator;
        _formattableStringParser = formattableStringParser;
        _componentRegistrationContext = componentRegistrationContext;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is ExpressionStringTemplateIdentifier or FormattableStringTemplateIdentifier;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        if (identifier is ExpressionStringTemplateIdentifier expressionStringTemplateIdentifier)
        {
            return new ExpressionStringTemplate(expressionStringTemplateIdentifier, _expressionStringEvaluator, _formattableStringParser, _componentRegistrationContext);
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

    public Task<Result> StartSession(CancellationToken cancellationToken)
    {
        _componentRegistrationContext.Placeholders.Clear();
        _componentRegistrationContext.Functions.Clear();

        return Task.FromResult(Result.Success());
    }
}
