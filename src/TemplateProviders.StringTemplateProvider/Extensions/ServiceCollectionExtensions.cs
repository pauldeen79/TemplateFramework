namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkStringTemplateProvider(this IServiceCollection services)
        => services
            .AddScoped<ITemplateProviderComponent, ProviderComponent>()
            .AddSingleton<IMember, TemplateFrameworkContextPlaceholderProcessor>()
            .AddSingleton<IMember, TemplateFrameworkContextFunction>()
            .AddSingleton<IMember, ComponentRegistrationContextFunction>()
            .AddScoped<ComponentRegistrationContext>();
}
