namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkCompiledTemplateProvider(this IServiceCollection services)
        => services
            .AddSingleton<ITemplateProviderComponent, ProviderComponent>();
}
