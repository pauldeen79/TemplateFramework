namespace TemplateFramework.Runtime.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkRuntime(this IServiceCollection services)
        => services.AddSingleton<IAssemblyService, PluginAssemblyService>();
}
