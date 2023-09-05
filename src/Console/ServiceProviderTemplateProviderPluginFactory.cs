namespace TemplateFramework.Console;

[ExcludeFromCodeCoverage]
public class ServiceProviderTemplateProviderPluginFactory : ITemplateProviderPluginFactory
{
    public IAssemblyService AssemblyService { get; set; } = default!;
    public IServiceProvider Provider { get; set; } = default!;

    public ITemplateProviderPlugin Create(string assemblyName, string className, string currentDirectory)
    {
        Guard.IsNotNull(assemblyName);
        Guard.IsNotNull(className);
        Guard.IsNotNull(currentDirectory);
        Guard.IsNotNull(AssemblyService);
        Guard.IsNotNull(Provider);

        var assembly = AssemblyService.GetAssembly(assemblyName, currentDirectory);
        var type = assembly.GetType(className);
        if (type is null)
        {
            throw new NotSupportedException($"Could not instanciate class name [{className}] from assembly [{assemblyName}]");
        }

        var plugin = new ServiceProviderCompiledTemplateFactory() { Provider = Provider }.Create(type) as ITemplateProviderPlugin;
        if (plugin is null)
        {
            throw new NotSupportedException($"Could not convert class name [{className}] from assembly [{assemblyName}] to an instance of type ${nameof(ITemplateProviderPlugin)}");
        }

        return plugin;
    }
}
