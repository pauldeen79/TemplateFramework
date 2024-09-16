namespace TemplateFramework.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFramework(this IServiceCollection services)
        => services
            .AddScoped<IFileSystem, FileSystem>()
            .AddScoped<ITemplateEngine, TemplateEngine>()
            .AddScoped<ITemplateInitializer, TemplateInitializer>()
            .AddScoped<ITemplateRenderer, MultipleStringContentBuilderTemplateRenderer>()
            .AddScoped<ITemplateRenderer, StringBuilderTemplateRenderer>()
            .AddScoped<IMultipleContentBuilderTemplateCreator<StringBuilder>, TypedMultipleCreator<StringBuilder>>()
            .AddScoped<IBuilderTemplateRenderer<StringBuilder>, TypedBuilderTemplateRenderer<StringBuilder>>()
            .AddScoped<IBuilderTemplateRenderer<StringBuilder>, TypedTextTransformTemplateRenderer>()
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
            .AddSingleton<ITemplateParameterConverter>(x => new ViewModelTemplateParameterConverter(() => x.GetServices<IViewModel>())) // Add support for ViewModels
            ;
}
