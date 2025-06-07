namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ExpressionStringTemplate : IBuilderTemplate<StringBuilder>
{
    private readonly ExpressionStringTemplateIdentifier _expressionStringTemplateIdentifier;
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ComponentRegistrationContext _componentRegistrationContext;
    private readonly IDictionary<string, object?> _parametersDictionary;

    public ExpressionStringTemplate(
        ExpressionStringTemplateIdentifier expressionStringTemplateIdentifier,
        IExpressionEvaluator expressionEvaluator,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(expressionStringTemplateIdentifier);
        Guard.IsNotNull(expressionEvaluator);
        Guard.IsNotNull(componentRegistrationContext);

        _expressionStringTemplateIdentifier = expressionStringTemplateIdentifier;
        _expressionEvaluator = expressionEvaluator;
        _componentRegistrationContext = componentRegistrationContext;

        _parametersDictionary = new Dictionary<string, object?>();
    }

    public async Task<Result> Render(StringBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkStringContext(_parametersDictionary, _componentRegistrationContext, false);
        var result = await _expressionEvaluator.EvaluateTypedAsync<GenericFormattableString>("$\"" + _expressionStringTemplateIdentifier.Template + "\"", new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(_expressionStringTemplateIdentifier.FormatProvider), new Dictionary<string, Task<Result<object?>>> { { "context", Task.FromResult(Result.Success<object?>(context)) } }, cancellationToken).ConfigureAwait(false);

        if (result.IsSuccessful() && result.Value is not null)
        {
            builder.Append(result.Value);
        }

        return result;
    }
}
