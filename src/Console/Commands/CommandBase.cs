namespace TemplateFramework.Console.Commands;

internal static class CommandBase
{
    [ExcludeFromCodeCoverage]
    internal static void Watch(CommandLineApplication app, CommandOption<string> watchOption, string fileName, Action action)
    {
        action();

        if (!watchOption.HasValue())
        {
            return;
        }

        if (!File.Exists(fileName))
        {
            app.Out.WriteLine($"Error: Could not find file [{fileName}]. Could not watch file for changes.");
            return;
        }

        app.Out.WriteLine($"Watching file [{fileName}] for changes...");
        var previousLastWriteTime = new FileInfo(fileName).LastWriteTime;
        while (true)
        {
            if (!File.Exists(fileName))
            {
                app.Out.WriteLine($"Error: Could not find file [{fileName}]. Could not watch file for changes.");
                return;
            }
            var currentLastWriteTime = new FileInfo(fileName).LastWriteTime;
            if (currentLastWriteTime != previousLastWriteTime)
            {
                previousLastWriteTime = currentLastWriteTime;
                action();
            }
            Thread.Sleep(1000);
        }
    }
}
