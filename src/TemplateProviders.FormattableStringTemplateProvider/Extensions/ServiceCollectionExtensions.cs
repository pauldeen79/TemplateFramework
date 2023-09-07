namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkFormattableStringTemplateProvider(this IServiceCollection services)
        => services
            .AddScoped<ITemplateProviderComponent, ProviderComponent>()
            .AddScoped<IPlaceholderProcessor, TemplateFrameworkContextPlaceholderProcessor>()
            .AddScoped<ComponentRegistrationContext>();
}
