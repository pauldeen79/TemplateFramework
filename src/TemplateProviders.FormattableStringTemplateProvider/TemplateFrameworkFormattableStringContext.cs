namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public sealed class TemplateFrameworkFormattableStringContext
{
    public TemplateFrameworkFormattableStringContext(IDictionary<string, object?> parametersDictionary, IList<IPlaceholderProcessor> processors)
    {
        Guard.IsNotNull(parametersDictionary);
        Guard.IsNotNull(processors);

        ParametersDictionary = parametersDictionary;
        Processors = processors;
        ParameterNamesList = new List<string>();
    }

    public IDictionary<string, object?> ParametersDictionary { get; }
    public IList<IPlaceholderProcessor> Processors { get; }
    public IList<string> ParameterNamesList { get; }
}
