namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class ComponentRegistrationContext
{
    public IList<IPlaceholderProcessor> Processors { get; } = new List<IPlaceholderProcessor>();
}
