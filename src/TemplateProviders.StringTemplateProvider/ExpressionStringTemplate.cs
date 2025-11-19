namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ExpressionStringTemplate : IBuilderTemplate<StringBuilder>, ITemplateEngineContextContainer
{
    public ITemplateEngineContext Context { get; set; }

    private readonly ExpressionStringTemplateIdentifier _expressionStringTemplateIdentifier;
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

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

        Context = default!; // furhter on in the process, this will get filled
    }

    public async Task<Result> RenderAsync(StringBuilder builder, CancellationToken token)
    {
        Guard.IsNotNull(Context);
        Guard.IsNotNull(builder);

        var templateFrameworkStringContext = new TemplateFrameworkStringContext(Context.ParametersDictionary, _componentRegistrationContext, false);
        var result = await _expressionEvaluator.EvaluateAsync(_expressionStringTemplateIdentifier.Template, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(_expressionStringTemplateIdentifier.FormatProvider), new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(templateFrameworkStringContext)) } }, token).ConfigureAwait(false);

        if (result.IsSuccessful() && result.Value is not null)
        {
            builder.Append(result.Value);
        }

        return result;
    }
}
