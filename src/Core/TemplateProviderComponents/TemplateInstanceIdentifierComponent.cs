namespace TemplateFramework.Core.TemplateProviderComponents;

public class TemplateInstanceIdentifierComponent : ITemplateProviderComponent
{
    public Result<object> Create(ITemplateIdentifier identifier)
    {
        if (identifier is not TemplateInstanceIdentifier templateInstanceIdentifier)
        {
            return Result.Continue<object>();
        }

        return Result.Success(templateInstanceIdentifier.Instance);
    }
}
