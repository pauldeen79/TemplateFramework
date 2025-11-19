namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ProviderComponent : ITemplateProviderComponent, ISessionAwareComponent
{
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

    public ProviderComponent(
        IExpressionEvaluator expressionEvaluator,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(expressionEvaluator);
        Guard.IsNotNull(componentRegistrationContext);

        _expressionEvaluator = expressionEvaluator;
        _componentRegistrationContext = componentRegistrationContext;
    }

    public bool Supports(ITemplateIdentifier identifier) => identifier is ExpressionStringTemplateIdentifier or FormattableStringTemplateIdentifier;

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        if (identifier is ExpressionStringTemplateIdentifier expressionStringTemplateIdentifier)
        {
            return new ExpressionStringTemplate(expressionStringTemplateIdentifier, _expressionEvaluator, _componentRegistrationContext);
        }
        else if (identifier is FormattableStringTemplateIdentifier formattableStringTemplateIdentifier)
        {
            return new FormattableStringTemplate(formattableStringTemplateIdentifier, _expressionEvaluator, _componentRegistrationContext);
        }
        else
        {
            throw new NotSupportedException($"Identifier of type {identifier.GetType().FullName} is not supported");
        }
    }

    public Task<Result> StartSessionAsync(CancellationToken token)
    {
        _componentRegistrationContext.Expressions.Clear();
        _componentRegistrationContext.ClearFunctions();

        return Task.FromResult(Result.Success());
    }
}
