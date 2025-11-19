namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class FormattableStringTemplate : IParameterizedTemplate, IBuilderTemplate<StringBuilder>, ITemplateEngineContextContainer
{
    public ITemplateEngineContext Context { get; set; }

    private readonly FormattableStringTemplateIdentifier _formattableStringTemplateIdentifier;
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

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

        Context = default!; // furhter on in the process, this will get filled
    }

    public async Task<Result<ITemplateParameter[]>> GetParametersAsync(CancellationToken token)
    {
        Guard.IsNotNull(Context);

        var templateFrameworkStringContext = new TemplateFrameworkStringContext(Context.ParametersDictionary, _componentRegistrationContext, true);

        var result = await _expressionEvaluator.ParseAsync("$\"" + _formattableStringTemplateIdentifier.Template + "\"", new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(_formattableStringTemplateIdentifier.FormatProvider), new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(templateFrameworkStringContext)) } }, token).ConfigureAwait(false);
        if (!result.IsSuccessful())
        {
            return Result.FromExistingResult<ITemplateParameter[]>(result);
        }

        return Result.Success<ITemplateParameter[]>(templateFrameworkStringContext.ParameterNamesList
            .Select(x => new TemplateParameter(x, typeof(string)))
            .ToArray());
    }

    public async Task<Result> RenderAsync(StringBuilder builder, CancellationToken token)
    {
        Guard.IsNotNull(Context);
        Guard.IsNotNull(builder);

        var templateFrameworkStringContext = new TemplateFrameworkStringContext(Context.ParametersDictionary, _componentRegistrationContext, false);
        var result = await _expressionEvaluator.EvaluateTypedAsync<GenericFormattableString>("$\"" + _formattableStringTemplateIdentifier.Template + "\"", new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(_formattableStringTemplateIdentifier.FormatProvider), new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(templateFrameworkStringContext)) } }, token).ConfigureAwait(false);

        if (result.IsSuccessful() && result.Value is not null)
        {
            builder.Append(result.Value.ToString(_formattableStringTemplateIdentifier.FormatProvider));
        }

        return result;
    }

    public Task<Result> SetParameterAsync(string name, object? value, CancellationToken token)
        => Task.Run(() =>
        {
            Guard.IsNotNull(Context);

            Context.ParametersDictionary[name] = value;

            return Result.Success();
        }, token);
}
