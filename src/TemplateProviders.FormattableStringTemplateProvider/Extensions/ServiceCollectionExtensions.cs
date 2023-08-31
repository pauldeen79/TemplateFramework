namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkFormattableStringTemplateProvider(this IServiceCollection services)
        => services
            .AddScoped<ITemplateProviderComponent, ProviderComponent>()
            .AddSingleton<IPlaceholderProcessor, TemplateFrameworkContextPlaceholderProcessor>();
}
