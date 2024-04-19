namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class FormattableStringTemplate : IParameterizedTemplate, IStringBuilderTemplate
{
    private readonly FormattableStringTemplateIdentifier _formattableStringTemplateIdentifier;
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly ComponentRegistrationContext _componentRegistrationContext;
    private readonly IDictionary<string, object?> _parametersDictionary;
    
    public FormattableStringTemplate(
        FormattableStringTemplateIdentifier formattableStringTemplateIdentifier,
        IFormattableStringParser formattableStringParser,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(formattableStringTemplateIdentifier);
        Guard.IsNotNull(formattableStringParser);
        Guard.IsNotNull(componentRegistrationContext);

        _formattableStringTemplateIdentifier = formattableStringTemplateIdentifier;
        _formattableStringParser = formattableStringParser;
        _componentRegistrationContext = componentRegistrationContext;

        _parametersDictionary = new Dictionary<string, object?>();
    }

    public ITemplateParameter[] GetParameters()
    {
        var context = new TemplateFrameworkStringContext(_parametersDictionary, _componentRegistrationContext, true);
        
        _ = _formattableStringParser.Parse(_formattableStringTemplateIdentifier.Template, _formattableStringTemplateIdentifier.FormatProvider, context);
        
        return context.ParameterNamesList
            .Select(x => new TemplateParameter(x, typeof(string)))
            .ToArray();
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkStringContext(_parametersDictionary, _componentRegistrationContext, false);
        var result = _formattableStringParser.Parse(_formattableStringTemplateIdentifier.Template, _formattableStringTemplateIdentifier.FormatProvider, context).GetValueOrThrow();

        builder.Append(result.ToString(_formattableStringTemplateIdentifier.FormatProvider));
    }

    public void SetParameter(string name, object? value) => _parametersDictionary[name] = value;
}
