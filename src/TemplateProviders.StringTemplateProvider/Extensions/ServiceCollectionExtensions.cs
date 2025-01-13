namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkStringTemplateProvider(this IServiceCollection services)
        => services
            .AddScoped<ITemplateProviderComponent, ProviderComponent>()
            .AddScoped<IPlaceholder, TemplateFrameworkContextPlaceholderProcessor>()
            .AddScoped<IFunction, TemplateFrameworkContextFunction>()
            .AddScoped<IFunction, ComponentRegistrationContextFunction>()
            .AddScoped<ComponentRegistrationContext>();
}
