namespace TemplateFramework.Console;

[ExcludeFromCodeCoverage]
public class CommandLineProcessor : ICommandLineProcessor
{
    private readonly IEnumerable<ICommandLineCommand> _commands;

    public CommandLineProcessor(IEnumerable<ICommandLineCommand> commands)
    {
        Guard.IsNotNull(commands);
        _commands = commands;
    }

    public void Initialize(CommandLineApplication app)
    {
        foreach (var command in _commands)
        {
            command.Initialize(app);
        }
    }
}
