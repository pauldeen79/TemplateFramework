namespace TemplateFramework.Runtime.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkRuntime(this IServiceCollection services)
        => services
            .AddScoped<IAssemblyService, CustomAssemblyService>()
            .AddScoped<ITemplateFactory>(p => new ServiceProviderCompiledTemplateFactory(p))
            .AddScoped<ITemplateComponentRegistryPluginFactory, ServiceProviderTemplateComponentRegistryPluginFactory>()
            ;
}
