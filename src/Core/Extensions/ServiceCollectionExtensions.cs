namespace TemplateFramework.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFramework(this IServiceCollection services)
        => services
            .AddScoped<IFileSystem, FileSystem>()
            .AddScoped<ITemplateEngine, TemplateEngine>()
            .AddScoped<ITemplateInitializer, TemplateInitializer>()
            .AddScoped<ITemplateRenderer, MultipleContentTemplateRenderer>()
            .AddScoped<ITemplateRenderer, StringBuilderTemplateRenderer>()
            .AddScoped<IMultipleContentBuilderTemplateCreator, TypedMultipleCreator>()
            .AddScoped<IStringBuilderTemplateRenderer, TypedStringBuilderTemplateRenderer>()
            .AddScoped<IStringBuilderTemplateRenderer, TypedTextTransformTemplateRenderer>()
            .AddScoped<ITemplateInitializerComponent, ModelInitializerComponent>()
            .AddScoped<ITemplateInitializerComponent, ParameterInitializerComponent>()
            .AddScoped<ITemplateInitializerComponent, ContextInitializerComponent>()
            .AddScoped<ITemplateInitializerComponent, DefaultFilenameInitializerComponent>()
            .AddScoped<ITemplateInitializerComponent, ProviderPluginInitializerComponent>()
            .AddScoped<ITemplateParameterExtractorComponent, TypedExtractor>()
            .AddScoped<ITemplateParameterExtractorComponent, PropertyExtractor>()
            .AddScoped<IValueConverter, ValueConverter>()
            .AddScoped<ITemplateProvider, TemplateProvider>()
            .AddScoped<ITemplateProviderComponent, TemplateInstanceIdentifierComponent>()
            .AddScoped<ITemplateProviderComponent, TemplateTypeIdentifierComponent>()
            .AddScoped<IRetryMechanism, RetryMechanism>()
            .AddScoped<ITemplateParameterExtractor, TemplateParameterExtractor>()
            .AddScoped<ISingleContentTemplateRenderer, StringBuilderTemplateRenderer>() // also register using its own type, so we can render a single template from  multiple content template renderer
            ;
}
