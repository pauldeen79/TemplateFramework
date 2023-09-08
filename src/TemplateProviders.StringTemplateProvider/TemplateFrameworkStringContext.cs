namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class TemplateFrameworkStringContext
{
    public TemplateFrameworkStringContext(IDictionary<string, object?> parametersDictionary, ComponentRegistrationContext context, bool getParametersOnly)
    {
        Guard.IsNotNull(parametersDictionary);
        Guard.IsNotNull(context);

        ParametersDictionary = parametersDictionary;
        Context = context;
        ParameterNamesList = new List<string>();
        GetParametersOnly = getParametersOnly;
    }

    public IDictionary<string, object?> ParametersDictionary { get; }
    public ComponentRegistrationContext Context { get; }
    public IList<string> ParameterNamesList { get; }
    public bool GetParametersOnly { get; }
}
