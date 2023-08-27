namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkFormattableStringTemplateProvider(this IServiceCollection services)
        => services
            .AddSingleton<ITemplateProviderComponent, ProviderComponent>()
            .AddSingleton<IPlaceholderProcessor, TemplateFrameworkContextPlaceholderProcessor>();
}
