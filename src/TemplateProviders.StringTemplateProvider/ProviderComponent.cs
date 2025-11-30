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

    public Result<object> Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        if (identifier is ExpressionStringTemplateIdentifier expressionStringTemplateIdentifier)
        {
            return Result.Success<object>(new ExpressionStringTemplate(expressionStringTemplateIdentifier, _expressionEvaluator, _componentRegistrationContext));
        }

        if (identifier is FormattableStringTemplateIdentifier formattableStringTemplateIdentifier)
        {
            return Result.Success<object>(new FormattableStringTemplate(formattableStringTemplateIdentifier, _expressionEvaluator, _componentRegistrationContext));
        }

        return Result.Continue<object>();
    }

    public Task<Result> StartSessionAsync(CancellationToken token)
    {
        _componentRegistrationContext.Expressions.Clear();
        _componentRegistrationContext.ClearFunctions();

        return Task.FromResult(Result.Success());
    }
}
