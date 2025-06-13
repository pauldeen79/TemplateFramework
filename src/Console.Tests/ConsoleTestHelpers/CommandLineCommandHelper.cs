namespace TemplateFramework.Console.Tests.ConsoleTestHelpers;

internal static class CommandLineCommandHelper
{
    internal static async Task<string> ExecuteCommandAsync<T>(T sut, params string[] arguments)
        where T : ICommandLineCommand => await ExecuteCommandAsync(() => sut, arguments).ConfigureAwait(false);

    internal static async Task<string> ExecuteCommandAsync<T>(Func<T> sutCreateDelegate, params string[] arguments)
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
        await app.Commands.First().ExecuteAsync(arguments).ConfigureAwait(false);

        await writer.FlushAsync().ConfigureAwait(false);
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    internal static async Task<string> ExecuteCommandAsync(Func<CommandLineApplication, Task> appDelegate)
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        using var app = new CommandLineApplication();
        app.Out = writer;
        app.Error = writer;

        // Act
        await appDelegate(app).ConfigureAwait(false);

        await writer.FlushAsync().ConfigureAwait(false);
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
