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
            ;
}
