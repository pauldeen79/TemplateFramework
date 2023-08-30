namespace TemplateFramework.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFramework(this IServiceCollection services)
        => services
            .AddSingleton<IFileSystem, FileSystem>()
            .AddSingleton<ITemplateEngine, TemplateEngine>()
            .AddSingleton<ITemplateInitializer, TemplateInitializer>()
            .AddSingleton<ITemplateRenderer, StringBuilderTemplateRenderer>()
            .AddSingleton<ITemplateRenderer, MultipleContentTemplateRenderer>()
            .AddSingleton<IMultipleContentBuilderTemplateCreator, TypedMultipleCreator>()
            .AddSingleton<IMultipleContentBuilderTemplateCreator, WrappedMultipleCreator>()
            .AddSingleton<IStringBuilderTemplateRenderer, TypedStringBuilderTemplateRenderer>()
            .AddSingleton<IStringBuilderTemplateRenderer, TypedTextTransformTemplateRenderer>()
            .AddSingleton<IStringBuilderTemplateRenderer, WrappedStringBuilderTemplateRenderer>()
            .AddSingleton<IStringBuilderTemplateRenderer, WrappedTextTransformTemplateRenderer>()
            .AddSingleton<ITemplateInitializerComponent, ModelInitializerComponent>()
            .AddSingleton<ITemplateInitializerComponent, ParameterInitializerComponent>()
            .AddSingleton<ITemplateInitializerComponent, ContextInitializerComponent>()
            .AddSingleton<ITemplateInitializerComponent, DefaultFilenameInitializerComponent>()
            .AddSingleton<ITemplateParameterExtractorComponent, TypedExtractor>()
            .AddSingleton<IValueConverter, ValueConverter>()
            .AddSingleton<ITemplateProvider, TemplateProvider>()
            .AddSingleton<ITemplateProviderComponent, TemplateInstanceIdentifierComponent>()
            .AddSingleton<IRetryMechanism, RetryMechanism>()
            .AddSingleton<ITemplateParameterExtractor, TemplateParameterExtractor>()
            .AddSingleton<ISingleContentTemplateRenderer, StringBuilderTemplateRenderer>() // also register using its own type, so we can render a single template from  multiple content template renderer
            ;
}
