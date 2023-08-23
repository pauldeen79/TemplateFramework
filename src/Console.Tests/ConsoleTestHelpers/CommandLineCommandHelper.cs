namespace TemplateFramework.Console.Tests.ConsoleTestHelpers;

internal static class CommandLineCommandHelper
{
    internal static string ExecuteCommand<T>(Func<T> sutCreateDelegate, params string[] arguments)
        where T : ICommandLineCommand
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        using var app = new CommandLineApplication();
        app.Out = writer;
        app.Error = writer;
        var sut = sutCreateDelegate.Invoke();
        sut.Initialize(app);

        // Act
        app.Commands.First().Execute(arguments);

        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    internal static string ExecuteCommand(Action<CommandLineApplication> appDelegate)
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        using var app = new CommandLineApplication();
        app.Out = writer;
        app.Error = writer;

        // Act
        appDelegate(app);

        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
