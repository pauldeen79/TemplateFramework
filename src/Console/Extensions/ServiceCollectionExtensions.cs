namespace TemplateFramework.Console.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateCommands(this IServiceCollection instance)
        => instance
            .AddScoped<IUserInput, UserInput>()
            .AddScoped<ICommandLineProcessor, CommandLineProcessor>()
            .AddScoped<ICommandLineCommand, VersionCommand>()
            .AddScoped<ICommandLineCommand, CodeGenerationAssemblyCommand>()
            .AddScoped<ICommandLineCommand, RunTemplateCommand>();
}
