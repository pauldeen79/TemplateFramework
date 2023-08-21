namespace TemplateFramework.Console.Commands;

[ExcludeFromCodeCoverage]
public abstract class CommandBase : ICommandLineCommand
{
    internal ICodeGenerationAssembly _codeGenerationAssembly { get; }
    internal IClipboard _clipboard { get; }

    protected CommandBase(ICodeGenerationAssembly codeGenerationAssembly, IClipboard clipboard)
    {
        Guard.IsNotNull(codeGenerationAssembly);
        Guard.IsNotNull(clipboard);

        _codeGenerationAssembly = codeGenerationAssembly;
        _clipboard = clipboard;
    }

    protected static void Watch(CommandLineApplication app, CommandOption<string> watchOption, string fileName, Action action)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(watchOption);
        Guard.IsNotNull(fileName);
        Guard.IsNotNull(action);

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

    protected static string? GetCurrentDirectory(CommandOption<string> currentDirectoryOption, string assemblyName)
    {
        Guard.IsNotNull(currentDirectoryOption);
        Guard.IsNotNull(assemblyName);

        if (currentDirectoryOption.HasValue())
        {
            return currentDirectoryOption.Value()!;
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

    protected static void WriteOutputToHost(CommandLineApplication app, string templateOutput, CommandOption<bool> bareOption)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(templateOutput);
        Guard.IsNotNull(bareOption);

        if (!bareOption.HasValue())
        {
            app.Out.WriteLine("Code generation output:");
        }

        app.Out.WriteLine(templateOutput);
    }

    protected void WriteOutput(CommandLineApplication app, MultipleContentBuilderEnvironment generationEnvironment, string basePath, CommandOption<bool> bareOption, CommandOption<bool> clipboardOption, bool dryRun)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(basePath);
        Guard.IsNotNull(bareOption);
        Guard.IsNotNull(clipboardOption);

        if (!dryRun)
        {
            if (!bareOption.HasValue())
            {
                var dir = string.IsNullOrEmpty(basePath)
                    ? Directory.GetCurrentDirectory()
                    : basePath;

                app.Out.WriteLine($"Written code generation output to path: {dir}");
            }
        }
        else if (clipboardOption.HasValue())
        {
            WriteOutputToClipboard(app, GenerateSingleOutput(generationEnvironment.Builder, basePath), bareOption);
        }
        else
        {
            WriteOutputToHost(app, GenerateSingleOutput(generationEnvironment.Builder, basePath), bareOption);
        }
    }

    protected void WriteOutputToClipboard(CommandLineApplication app, string templateOutput, CommandOption<bool> bareOption)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(templateOutput);
        Guard.IsNotNull(bareOption);

        _clipboard.SetText(templateOutput);

        if (!bareOption.HasValue())
        {
            app.Out.WriteLine("Copied code generation output to clipboard");
        }
    }

    protected static bool GetDryRun(CommandOption<bool> dryRunOption, CommandOption<bool> clipboardOption)
    {
        Guard.IsNotNull(dryRunOption);
        Guard.IsNotNull(clipboardOption);

        return dryRunOption.HasValue() || clipboardOption.HasValue();
    }

    protected static string GetDefaultFilename(CommandOption<string> defaultFilenameOption)
    {
        Guard.IsNotNull(defaultFilenameOption);

        return defaultFilenameOption.HasValue()
            ? defaultFilenameOption.Value()!
            : string.Empty;
    }

    protected static string GetBasePath(CommandOption<string> basePathOption)
    {
        Guard.IsNotNull(basePathOption);

        return basePathOption.HasValue()
            ? basePathOption.Value()!
            : string.Empty;
    }

    public abstract void Initialize(CommandLineApplication app);
}
