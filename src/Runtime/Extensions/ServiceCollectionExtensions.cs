namespace TemplateFramework.Runtime.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkRuntime(this IServiceCollection services)
        => services
            .AddScoped<IAssemblyService, PluginAssemblyService>()
            .AddScoped<IAssemblyService, CustomAssemblyService>()
            ;
}
