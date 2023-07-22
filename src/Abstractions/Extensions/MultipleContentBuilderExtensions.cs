namespace TemplateFramework.Abstractions.Extensions;

public static class MultipleContentBuilderExtensions
{
    public static IContentBuilder AddContent(this IMultipleContentBuilder instance)
        => instance.AddContent(string.Empty, false, null);

    public static IContentBuilder AddContent(this IMultipleContentBuilder instance, string filename)
        => instance.AddContent(filename, false, null);

    public static IContentBuilder AddContent(this IMultipleContentBuilder instance, string filename, bool skipWhenFileExists)
        => instance.AddContent(filename, skipWhenFileExists, null);
}
