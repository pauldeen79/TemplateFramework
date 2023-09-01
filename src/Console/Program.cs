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

        var dynamicCompiledTemplateFactory = new DynamicCompiledTemplateFactory();
        var serviceCollection = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkCompiledTemplateProvider()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkFormattableStringTemplateProvider()
            .AddTemplateFrameworkRuntime()
            .AddTemplateCommands()
            .AddSingleton<IAssemblyInfoContextService, MyAssemblyInfoContextService>()
            .AddSingleton<ICompiledTemplateFactory>(dynamicCompiledTemplateFactory);
        serviceCollection.InjectClipboard();
        using var provider = serviceCollection.BuildServiceProvider();
        dynamicCompiledTemplateFactory.Provider = provider;
        var processor = provider.GetRequiredService<ICommandLineProcessor>();
        processor.Initialize(app);

        return app.Execute(args);
    }

    public sealed class MyAssemblyInfoContextService : IAssemblyInfoContextService
    {
        public string[] GetExcludedAssemblies() => new[]
        {
            "System.Runtime",
            "System.Collections",
            "System.ComponentModel",
            "TemplateFramework.Abstractions",
            "TemplateFramework.Console",
            "TemplateFramework.Core",
            "TemplateFramework.Core.CodeGeneration",
            "TemplateFramework.Runtime",
            "TemplateFramework.TemplateProviders.ChildTemplateProvider",
            "TemplateFramework.TemplateProviders.CompiledTemplateProvider",
            "TemplateFramework.TemplateProviders.FormattableStringTemplateProvider",
            "CrossCutting.Common",
            "CrossCutting.Utilities.Parsers",
            "Microsoft.Extensions.DependencyInjection",
            "Microsoft.Extensions.DependencyInjection.Abstractions"
        };
    }
}
