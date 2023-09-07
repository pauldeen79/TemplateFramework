﻿namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public class FormattableStringTemplate : IParameterizedTemplate, IStringBuilderTemplate
{
    private readonly FormattableStringTemplateIdentifier _createFormattableStringTemplateRequest;
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly ComponentRegistrationContext _componentRegistrationContext;
    private readonly IDictionary<string, object?> _parametersDictionary;
    
    public FormattableStringTemplate(
        FormattableStringTemplateIdentifier createFormattableStringTemplateRequest,
        IFormattableStringParser formattableStringParser,
        ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(createFormattableStringTemplateRequest);
        Guard.IsNotNull(formattableStringParser);
        Guard.IsNotNull(componentRegistrationContext);

        _createFormattableStringTemplateRequest = createFormattableStringTemplateRequest;
        _formattableStringParser = formattableStringParser;
        _componentRegistrationContext = componentRegistrationContext;

        _parametersDictionary = new Dictionary<string, object?>();
    }

    public ITemplateParameter[] GetParameters()
    {
        var context = new TemplateFrameworkFormattableStringContext(_parametersDictionary, _componentRegistrationContext.Processors, true);
        
        _ = _formattableStringParser.Parse(_createFormattableStringTemplateRequest.Template, _createFormattableStringTemplateRequest.FormatProvider, context);
        
        return context.ParameterNamesList
            .Select(x => new TemplateParameter(x, typeof(string)))
            .ToArray();
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkFormattableStringContext(_parametersDictionary, _componentRegistrationContext.Processors, false);
        var result = _formattableStringParser.Parse(_createFormattableStringTemplateRequest.Template, _createFormattableStringTemplateRequest.FormatProvider, context).GetValueOrThrow();

        builder.Append(result);
    }

    public void SetParameter(string name, object? value) => _parametersDictionary[name] = value;
}
