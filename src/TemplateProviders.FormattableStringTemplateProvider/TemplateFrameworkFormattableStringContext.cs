namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public sealed class TemplateFrameworkFormattableStringContext
{
    public TemplateFrameworkFormattableStringContext(IDictionary<string, object?> parametersDictionary)
    {
        ParametersDictionary = parametersDictionary;
        ParameterNamesList = new List<string>();
    }

    public IDictionary<string, object?> ParametersDictionary { get; }
    public IList<string> ParameterNamesList { get; }
}
