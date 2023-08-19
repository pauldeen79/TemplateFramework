namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkChildTemplateProvider(this IServiceCollection services)
        => services
            .AddSingleton<ITemplateProviderComponent, ProviderComponent>();

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, Type modelType) where T : class, new()
        => services.AddSingleton<ITemplateCreator>(_ => new TemplateCreator<T>(modelType, null));

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, string name) where T : class, new()
        => services.AddSingleton<ITemplateCreator>(_ => new TemplateCreator<T>(null, name));

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, Type modelType, string name) where T : class, new()
        => services.AddSingleton<ITemplateCreator>(_ => new TemplateCreator<T>(modelType, name));
}
