namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public class FormattableStringTemplate : IParameterizedTemplate, IStringBuilderTemplate
{
    private readonly CreateFormattableStringTemplateRequest _createFormattableStringTemplateRequest;
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly IDictionary<string, object?> _parametersDictionary;

    public FormattableStringTemplate(
        CreateFormattableStringTemplateRequest createFormattableStringTemplateRequest,
        IFormattableStringParser formattableStringParser)
    {
        Guard.IsNotNull(createFormattableStringTemplateRequest);
        Guard.IsNotNull(formattableStringParser);

        _createFormattableStringTemplateRequest = createFormattableStringTemplateRequest;
        _formattableStringParser = formattableStringParser;

        _parametersDictionary = new Dictionary<string, object?>();
    }

    public ITemplateParameter[] GetParameters()
    {
        var context = new TemplateFrameworkFormattableStringContext(_parametersDictionary);
        
        _ = _formattableStringParser.Parse(_createFormattableStringTemplateRequest.Template, _createFormattableStringTemplateRequest.FormatProvider, context);
        
        return context.ParameterNamesList
            .Select(x => new TemplateParameter(x, typeof(string)))
            .ToArray();
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkFormattableStringContext(_parametersDictionary);
        var result = _formattableStringParser.Parse(_createFormattableStringTemplateRequest.Template, _createFormattableStringTemplateRequest.FormatProvider, context).GetValueOrThrow();

        builder.Append(result);
    }

    public void SetParameter(string name, object? value) => _parametersDictionary[name] = value;
}
