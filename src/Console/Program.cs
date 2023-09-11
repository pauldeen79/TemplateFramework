namespace TemplateFramework.Console;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static int Main(string[] args)
    {
        using var app = new CommandLineApplication
        {
            Name = "tf",
            Description = "TemplateFramework",
            UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue
        };
        app.HelpOption();

        var dynamicTemplateFactory = new ServiceProviderCompiledTemplateFactory();
        var dynamicComponentRegistryPluginFactory = new ServiceProviderTemplateComponentRegistryPluginFactory();
        var serviceCollection = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkCompiledTemplateProvider()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddTemplateFrameworkRuntime()
            .AddTemplateCommands()
            .AddSingleton<IAssemblyInfoContextService, MyAssemblyInfoContextService>()
            .AddSingleton<ITemplateFactory>(dynamicTemplateFactory)
            .AddSingleton<ITemplateComponentRegistryPluginFactory>(dynamicComponentRegistryPluginFactory);
        serviceCollection.InjectClipboard();
        using var provider = serviceCollection.BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        dynamicTemplateFactory.Provider = scope.ServiceProvider;
        dynamicComponentRegistryPluginFactory.AssemblyService = scope.ServiceProvider.GetRequiredService<IAssemblyService>();
        dynamicComponentRegistryPluginFactory.Provider = scope.ServiceProvider;
        var processor = scope.ServiceProvider.GetRequiredService<ICommandLineProcessor>();
        processor.Initialize(app);

        return app.Execute(args);
    }
}
