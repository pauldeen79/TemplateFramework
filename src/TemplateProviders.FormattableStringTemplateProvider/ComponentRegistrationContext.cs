namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public sealed class ComponentRegistrationContext
{
    public IList<IPlaceholderProcessor> Processors { get; } = new List<IPlaceholderProcessor>();
}
