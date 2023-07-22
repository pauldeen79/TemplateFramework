namespace TemplateFramework.Abstractions.Extensions;

public static class TemplateContextExtensions
{
    public static T? GetModelFromContextByType<T>(this ITemplateContext instance)
        => instance.GetModelFromContextByType<T>(null);

    public static T? GetContextByTemplateType<T>(this ITemplateContext instance)
        => instance.GetContextByTemplateType<T>(null);
}
