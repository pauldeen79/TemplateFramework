namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class FormattableStringTemplate : IParameterizedTemplate, IBuilderTemplate<StringBuilder>
{
    private readonly FormattableStringTemplateIdentifier _formattableStringTemplateIdentifier;
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ComponentRegistrationContext _componentRegistrationContext;
    private readonly Dictionary<string, object?> _parametersDictionary;

    public FormattableStringTemplate(
        FormattableStringTemplateIdentifier formattableStringTemplateIdentifier,
        IExpressionEvaluator expressionEvaluator,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(formattableStringTemplateIdentifier);
        Guard.IsNotNull(expressionEvaluator);
        Guard.IsNotNull(componentRegistrationContext);

        _formattableStringTemplateIdentifier = formattableStringTemplateIdentifier;
        _expressionEvaluator = expressionEvaluator;
        _componentRegistrationContext = componentRegistrationContext;

        _parametersDictionary = [];
    }

    public async Task<Result<ITemplateParameter[]>> GetParametersAsync(CancellationToken cancellationToken)
    {
        var context = new TemplateFrameworkStringContext(_parametersDictionary, _componentRegistrationContext, true);

        _ = await _expressionEvaluator.ParseAsync("$\"" + _formattableStringTemplateIdentifier.Template + "\"", new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(_formattableStringTemplateIdentifier.FormatProvider), new Dictionary<string, Task<Result<object?>>> { { "context", Task.FromResult(Result.Success<object?>(context)) } }, cancellationToken).ConfigureAwait(false);

        return Result.Success<ITemplateParameter[]>(context.ParameterNamesList
            .Select(x => new TemplateParameter(x, typeof(string)))
            .ToArray());
    }

    public async Task<Result> Render(StringBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkStringContext(_parametersDictionary, _componentRegistrationContext, false);
        var result = await _expressionEvaluator.EvaluateTypedAsync<GenericFormattableString>("$\"" + _formattableStringTemplateIdentifier.Template + "\"", new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(_formattableStringTemplateIdentifier.FormatProvider), new Dictionary<string, Task<Result<object?>>> { { "context", Task.FromResult(Result.Success<object?>(context)) } }, cancellationToken).ConfigureAwait(false);

        if (result.IsSuccessful() && result.Value is not null)
        {
            builder.Append(result.Value.ToString(_formattableStringTemplateIdentifier.FormatProvider));
        }

        return result;
    }

    public Task<Result> SetParameterAsync(string name, object? value, CancellationToken cancellationToken)
        => Task.Run(() =>
        {
            _parametersDictionary[name] = value;
            return Result.Success();
        }, cancellationToken);
}
