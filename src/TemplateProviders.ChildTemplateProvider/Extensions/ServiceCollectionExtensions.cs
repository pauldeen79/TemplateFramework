namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkChildTemplateProvider(this IServiceCollection services)
        => services
            .AddScoped<ITemplateProviderComponent, ProviderComponent>();

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, Type modelType) where T : class
        => services
            .AddTransient<T>()
            .AddScoped<ITemplateCreator>(provider => new TemplateCreator<T>(() => provider.GetRequiredService<T>(), modelType, null));

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, Type modelType, Func<IServiceProvider, T> templateFactory) where T : class
        => services
            .AddScoped<ITemplateCreator>(provider => new TemplateCreator<T>(() => templateFactory(provider), modelType, null));

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, string name) where T : class
        => services
            .AddTransient<T>()
            .AddScoped<ITemplateCreator>(provider => new TemplateCreator<T>(() => provider.GetRequiredService<T>(), null, name));

    public static IServiceCollection AddChildTemplate<T>(this IServiceCollection services, string name, Func<IServiceProvider, T> templateFactory) where T : class
        => services
            .AddScoped<ITemplateCreator>(provider => new TemplateCreator<T>(() => templateFactory(provider), null, name));

    public static IServiceCollection AddViewModel<T>(this IServiceCollection services) where T : class, IViewModel
        => services
            .AddTransient<IViewModel, T>();
}
