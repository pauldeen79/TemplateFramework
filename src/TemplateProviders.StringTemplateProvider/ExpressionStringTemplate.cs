namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ExpressionStringTemplate : IBuilderTemplate<StringBuilder>
{
    private readonly ExpressionStringTemplateIdentifier _expressionStringTemplateIdentifier;
    private readonly IExpressionStringParser _expressionStringParser;
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly ComponentRegistrationContext _componentRegistrationContext;
    private readonly IDictionary<string, object?> _parametersDictionary;
    
    public ExpressionStringTemplate(
        ExpressionStringTemplateIdentifier expressionStringTemplateIdentifier,
        IExpressionStringParser expressionStringParser,
        IFormattableStringParser formattableStringParser,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(expressionStringTemplateIdentifier);
        Guard.IsNotNull(expressionStringParser);
        Guard.IsNotNull(formattableStringParser);
        Guard.IsNotNull(componentRegistrationContext);

        _expressionStringTemplateIdentifier = expressionStringTemplateIdentifier;
        _expressionStringParser = expressionStringParser;
        _formattableStringParser = formattableStringParser;
        _componentRegistrationContext = componentRegistrationContext;

        _parametersDictionary = new Dictionary<string, object?>();
    }

    public Task<Result> Render(StringBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkStringContext(_parametersDictionary, _componentRegistrationContext, false);
        var result = _expressionStringParser.Parse(_expressionStringTemplateIdentifier.Template, _expressionStringTemplateIdentifier.FormatProvider, context, _formattableStringParser);

        if (result.IsSuccessful())
        {
            builder.Append(result.Value);
        }

        return Task.FromResult((Result)result);
    }
}
