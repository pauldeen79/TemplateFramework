namespace TemplateFramework.Core.TemplateProviderComponents;

public class TemplateTypeIdentifierComponent : ITemplateProviderComponent
{
    public Result<object> Create(ITemplateIdentifier identifier)
    {
        if (identifier is not TemplateTypeIdentifier templateTypeIdentifier)
        {
            return Result.Continue<object>();
        }

        return Result.Success(templateTypeIdentifier.TemplateFactory.Create(templateTypeIdentifier.Type));
    }
}
