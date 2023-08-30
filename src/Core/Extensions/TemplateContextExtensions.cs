namespace TemplateFramework.Core.Extensions;

public static class TemplateContextExtensions
{
    public static ITemplateContext WithTemplate(this ITemplateContext instance, object template)
    {
        Guard.IsNotNull(template);

        return new TemplateContext(
            instance.Engine,
            instance.Provider,
            instance.DefaultFilename,
            instance.Identifier,
            template,
            instance.Model,
            instance.ParentContext,
            instance.IterationNumber,
            instance.IterationCount
            );
    }
}
