namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class TestFormattableStringTemplate : IParameterizedTemplate, IBuilderTemplate<StringBuilder>, ISessionAwareComponent
{
    private readonly Dictionary<string, object?> _parameterValues = [];
    private readonly IExpressionEvaluator _expressionEvaluator;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

    public TestFormattableStringTemplate(IExpressionEvaluator expressionEvaluator, ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(expressionEvaluator);
        Guard.IsNotNull(componentRegistrationContext);

        _expressionEvaluator = expressionEvaluator;
        _componentRegistrationContext = componentRegistrationContext;
    }

    const string Template = @"        [Fact]
        public void {UnittestName}()
        {{
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.{MethodName}();

            // Assert
            //TODO
        }}";

    public Task<Result<ITemplateParameter[]>> GetParametersAsync(CancellationToken cancellationToken)
        => new FormattableStringTemplate(new FormattableStringTemplateIdentifier(Template, CultureInfo.CurrentCulture), _expressionEvaluator, _componentRegistrationContext).GetParametersAsync(cancellationToken);

    public async Task<Result> RenderAsync(StringBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkStringContext(_parameterValues, _componentRegistrationContext, false);

        var result = await _expressionEvaluator.EvaluateTypedAsync<GenericFormattableString>("$\"" + Template + "\"", new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.CurrentCulture), new Dictionary<string, Task<Result<object?>>> { { "context", Task.FromResult(Result.Success<object?>(context)) } }, cancellationToken).ConfigureAwait(false);

        if (result.IsSuccessful() && result.Value is not null)
        {
            builder.Append(result.Value);
        }

        return result;
    }

    public Task<Result> SetParameterAsync(string name, object? value, CancellationToken cancellationToken)
        => Task.Run(() =>
        {
            //TODO: Find out why this is called twice when running the console app
            _parameterValues[name] = value;
            //_parameterValues.Add(name, value);
            return Result.Success();
        }, cancellationToken);

    public Task<Result> StartSessionAsync(CancellationToken cancellationToken)
    {
        _parameterValues.Clear();

        return Task.FromResult(Result.Success());
    }
}
