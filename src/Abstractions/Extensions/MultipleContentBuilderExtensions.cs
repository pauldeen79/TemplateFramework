namespace TemplateFramework.Abstractions.Extensions;

public static class MultipleContentBuilderExtensions
{
    public static IContentBuilder<T> AddContent<T>(this IMultipleContentBuilder<T> instance) where T : class
        => instance.AddContent(string.Empty, false, null);

    public static IContentBuilder<T> AddContent<T>(this IMultipleContentBuilder<T> instance, string filename) where T : class
        => instance.AddContent(filename, false, null);

    public static IContentBuilder<T> AddContent<T>(this IMultipleContentBuilder<T> instance, string filename, bool skipWhenFileExists) where T : class
        => instance.AddContent(filename, skipWhenFileExists, null);
}
