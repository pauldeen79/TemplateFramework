namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkChildTemplateProvider(this IServiceCollection services)
        => services
            .AddSingleton<ITemplateProviderComponent, ProviderComponent>();

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, Type modelType) where T : class, new()
        => services
            .AddSingleton<ITemplateCreator>(provider => new TemplateCreator<T>(() => provider.GetRequiredService<T>(), modelType, null))
            .AddTransient<T>();

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, string name) where T : class, new()
        => services
            .AddSingleton<ITemplateCreator>(provider => new TemplateCreator<T>(() => provider.GetRequiredService<T>(), null, name))
            .AddTransient<T>();

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, Type modelType, string name) where T : class, new()
        => services
            .AddSingleton<ITemplateCreator>(provider => new TemplateCreator<T>(() => provider.GetRequiredService<T>(), modelType, name))
            .AddTransient<T>();
}
