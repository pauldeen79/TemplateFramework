namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class ComponentRegistrationContext
{
    public IList<IPlaceholder> Placeholders { get; } = [];
    public IList<IFunction> Functions { get; } = [];
}
