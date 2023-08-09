namespace TemplateFramework.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFramework(this IServiceCollection services)
        => services
            .AddSingleton<IFileSystem, FileSystem>()
            .AddSingleton<ITemplateEngine, TemplateEngine>()
            .AddSingleton<ITemplateInitializer, DefaultTemplateInitializer>()
            .AddSingleton<ITemplateRenderer, StringBuilderTemplateRenderer>()
            .AddSingleton<ITemplateRenderer, MultipleContentTemplateRenderer>()
            .AddSingleton<IMultipleContentBuilderTemplateCreator, TypedMultipleCreator>()
            .AddSingleton<IMultipleContentBuilderTemplateCreator, WrappedMultipleCreator>()
            .AddSingleton<IStringBuilderTemplateRenderer, TypedStringBuilderTemplateRenderer>()
            .AddSingleton<IStringBuilderTemplateRenderer, TypedTextTransformTemplateRenderer>()
            .AddSingleton<ISingleContentTemplateRenderer, StringBuilderTemplateRenderer>() // also register using its own type, so we can render a single template from  multiple content template renderer
            ;
}
