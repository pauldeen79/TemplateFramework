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

        var serviceCollection = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkCompiledTemplateProvider()
            .AddTemplateFrameworkRuntime()
            .AddTemplateCommands();
        serviceCollection.InjectClipboard();
        using var provider = serviceCollection.BuildServiceProvider();
        var processor = provider.GetRequiredService<ICommandLineProcessor>();
        processor.Initialize(app);
        return app.Execute(args);
    }
}
