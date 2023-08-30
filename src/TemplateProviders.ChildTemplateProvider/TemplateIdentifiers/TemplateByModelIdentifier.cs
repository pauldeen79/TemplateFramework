namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.TemplateIdentifiers;

public sealed class TemplateByModelIdentifier : ITemplateIdentifier
{
    public TemplateByModelIdentifier(object? model)
    {
        Model = model;
    }

    public object? Model { get; }
}
