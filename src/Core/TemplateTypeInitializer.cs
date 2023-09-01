namespace TemplateFramework.Core;

public sealed class TemplateTypeIdentifier : ITemplateIdentifier
{
    public TemplateTypeIdentifier(Type type, ITemplateFactory templateFactory)
    {
        Guard.IsNotNull(type);
        Guard.IsNotNull(templateFactory);

        Type = type;
        TemplateFactory = templateFactory;
    }

    public Type Type { get; }
    public ITemplateFactory TemplateFactory { get; }
}
