namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkChildTemplateProvider(this IServiceCollection services)
        => services
            .AddSingleton<ITemplateProviderComponent, ProviderComponent>();
}
