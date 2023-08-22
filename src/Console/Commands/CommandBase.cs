namespace TemplateFramework.Console.Commands;

public abstract class CommandBase : ICommandLineCommand
{
    internal IClipboard _clipboard { get; }
    protected IFileSystem FileSystem { get; }
    protected bool Abort { get; set; }

    protected CommandBase(IClipboard clipboard, IFileSystem fileSystem)
    {
        Guard.IsNotNull(clipboard);
        Guard.IsNotNull(fileSystem);

        _clipboard = clipboard;
        FileSystem = fileSystem;
    }

    protected void Watch(CommandLineApplication app, bool watch, string fileName, Action action)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(fileName);
        Guard.IsNotNull(action);

        action();

        if (!watch)
        {
            return;
        }

        if (!FileSystem.FileExists(fileName))
        {
            app.Out.WriteLine($"Error: Could not find file [{fileName}]. Could not watch file for changes.");
            return;
        }

        app.Out.WriteLine($"Watching file [{fileName}] for changes...");
        var previousLastWriteTime = FileSystem.GetFileLastWriteTime(fileName);
        while (!Abort)
        {
            if (!FileSystem.FileExists(fileName))
            {
                app.Out.WriteLine($"Error: Could not find file [{fileName}]. Could not watch file for changes.");
                return;
            }

            var currentLastWriteTime = FileSystem.GetFileLastWriteTime(fileName);
            if (currentLastWriteTime != previousLastWriteTime)
            {
                previousLastWriteTime = currentLastWriteTime;
                action();
            }

            Thread.Sleep(1000);
        }
    }

    protected static string? GetCurrentDirectory(string? currentDirectory, string assemblyName)
    {
        Guard.IsNotNull(assemblyName);

        if (!string.IsNullOrEmpty(currentDirectory))
        {
            return currentDirectory;
        }

        return assemblyName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase)
            ? Path.GetDirectoryName(assemblyName)
            : string.Empty;
    }

    protected static string GenerateSingleOutput(IMultipleContentBuilder builder, string basePath)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(basePath);

        var stringBuilder = new StringBuilder();
        var output = builder.Build();

        foreach (var content in output.Contents)
        {
            var path = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(content.Filename)
                ? content.Filename
                : Path.Combine(basePath, content.Filename);

            stringBuilder.Append(path);
            stringBuilder.AppendLine(":");
            stringBuilder.AppendLine(content.Contents);
        }

        return stringBuilder.ToString();
    }

    protected static void WriteOutputToHost(CommandLineApplication app, string templateOutput, bool bare)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(templateOutput);

        if (!bare)
        {
            app.Out.WriteLine("Code generation output:");
        }

        app.Out.WriteLine(templateOutput);
    }

    protected void WriteOutput(CommandLineApplication app, MultipleContentBuilderEnvironment generationEnvironment, string basePath, bool bare, bool clipboard, bool dryRun)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(basePath);

        if (!dryRun)
        {
            if (!bare)
            {
                var dir = string.IsNullOrEmpty(basePath)
                    ? Directory.GetCurrentDirectory()
                    : basePath;

                app.Out.WriteLine($"Written code generation output to path: {dir}");
            }
        }
        else if (clipboard)
        {
            WriteOutputToClipboard(app, GenerateSingleOutput(generationEnvironment.Builder, basePath), bare);
        }
        else
        {
            WriteOutputToHost(app, GenerateSingleOutput(generationEnvironment.Builder, basePath), bare);
        }
    }

    protected void WriteOutputToClipboard(CommandLineApplication app, string templateOutput, bool bare)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(templateOutput);

        _clipboard.SetText(templateOutput);

        if (!bare)
        {
            app.Out.WriteLine("Copied code generation output to clipboard");
        }
    }

    protected static bool GetDryRun(bool dryRun, bool clipboard) => dryRun || clipboard;

    protected static string GetDefaultFilename(string? defaultFilename) => defaultFilename ?? string.Empty;

    protected static string GetBasePath(string? basePath) => basePath ?? string.Empty;

    public abstract void Initialize(CommandLineApplication app);
}
