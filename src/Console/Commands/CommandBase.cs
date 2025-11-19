namespace TemplateFramework.Console.Commands;

public abstract class CommandBase : ICommandLineCommand
{
    internal IClipboard _clipboard { get; }
    protected IFileSystem FileSystem { get; }
    protected IUserInput UserInput { get; }

    // Added for unit testing purpose only (is not changed by real implementations)
    protected bool Abort { get; set; }
    protected int SleepTimeInMs { get; set; } = 1000;

    protected CommandBase(IClipboard clipboard, IFileSystem fileSystem, IUserInput userInput)
    {
        Guard.IsNotNull(clipboard);
        Guard.IsNotNull(fileSystem);
        Guard.IsNotNull(userInput);

        _clipboard = clipboard;
        FileSystem = fileSystem;
        UserInput = userInput;
    }

    protected async Task Watch(CommandLineApplication app, bool watch, string filename, Func<Task> action, CancellationToken token)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(filename);
        Guard.IsNotNull(action);

        if (watch && !FileSystem.FileExists(filename))
        {
            await app.Out.WriteLineAsync($"Error: Could not find file [{filename}]. Could not watch file for changes.").ConfigureAwait(false);
            return;
        }

        await action().ConfigureAwait(false);

        if (!watch)
        {
            return;
        }

        await app.Out.WriteLineAsync($"Watching file [{filename}] for changes...").ConfigureAwait(false);
        var previousLastWriteTime = FileSystem.GetFileLastWriteTime(filename);
        while (!Abort)
        {
            if (!FileSystem.FileExists(filename))
            {
                await app.Out.WriteLineAsync($"Error: Could not find file [{filename}]. Could not watch file for changes.").ConfigureAwait(false);
                break;
            }

            var currentLastWriteTime = FileSystem.GetFileLastWriteTime(filename);
            if (currentLastWriteTime != previousLastWriteTime)
            {
                previousLastWriteTime = currentLastWriteTime;
                await action().ConfigureAwait(false);
            }

            await Task.Delay(SleepTimeInMs, token).ConfigureAwait(false);
        }
    }

    protected static string? GetCurrentDirectory(string? currentDirectory, string? assemblyName)
    {
        if (string.IsNullOrEmpty(assemblyName))
        {
            return string.Empty;
        }

        if (!string.IsNullOrEmpty(currentDirectory))
        {
            return currentDirectory;
        }

        return assemblyName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase)
            ? Path.GetDirectoryName(assemblyName)
            : string.Empty;
    }

    protected static string GenerateSingleOutput(IMultipleContentBuilder<StringBuilder> builder, string basePath)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(basePath);

        var stringBuilder = new StringBuilder();
        var output = builder.Build();

        stringBuilder.AppendMultipleContents(output, basePath);

        return stringBuilder.ToString();
    }

    protected static async Task WriteOutputToHost(CommandLineApplication app, string templateOutput, bool bare)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(templateOutput);

        if (!bare)
        {
            await app.Out.WriteLineAsync("Code generation output:").ConfigureAwait(false);
        }

        await app.Out.WriteLineAsync(templateOutput).ConfigureAwait(false);
    }

    protected async Task WriteOutput(CommandLineApplication app, MultipleContentBuilderEnvironment<StringBuilder> generationEnvironment, string basePath, bool bare, bool clipboard, bool dryRun, CancellationToken token)
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

                await app.Out.WriteLineAsync($"Written code generation output to path: {dir}").ConfigureAwait(false);
            }
        }
        else if (clipboard)
        {
            await WriteOutputToClipboard(app, GenerateSingleOutput(generationEnvironment.Builder, basePath), bare, token).ConfigureAwait(false);
        }
        else
        {
            await WriteOutputToHost(app, GenerateSingleOutput(generationEnvironment.Builder, basePath), bare).ConfigureAwait(false);
        }
    }

    protected async Task WriteOutputToClipboard(CommandLineApplication app, string templateOutput, bool bare, CancellationToken token)
    {
        Guard.IsNotNull(app);
        Guard.IsNotNull(templateOutput);

        await _clipboard.SetTextAsync(templateOutput, token).ConfigureAwait(false);

        if (!bare)
        {
            await app.Out.WriteLineAsync("Copied code generation output to clipboard").ConfigureAwait(false);
        }
    }

    protected KeyValuePair<string, object?>[] GetInteractiveParameterValues(ITemplateParameter[] templateParameters)
    {
        Guard.IsNotNull(templateParameters);

        var list = new List<KeyValuePair<string, object?>>();

        foreach (var templateParameter in templateParameters)
        {
            var value = UserInput.GetValue(templateParameter);
            list.Add(new KeyValuePair<string, object?>(templateParameter.Name, value));
        }

        return list.ToArray();
    }

    protected static void AppendParameters(MultipleContentBuilderEnvironment<StringBuilder> generationEnvironment, string defaultFilename, ITemplateParameter[] templateParameters)
    {
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(templateParameters);

        var content = generationEnvironment.Builder.AddContent(defaultFilename);
        foreach (var parameter in templateParameters)
        {
            content.Builder.AppendLine(CultureInfo.CurrentCulture, $"{parameter.Name} ({parameter.Type.FullName})");
        }
    }

    protected static bool GetDryRun(bool dryRun, bool clipboard) => dryRun || clipboard;

    protected static string GetDefaultFilename(string? defaultFilename) => defaultFilename ?? string.Empty;

    protected static string GetBasePath(string? basePath) => basePath ?? string.Empty;

    protected static KeyValuePair<string, object?>[] GetParameters(CommandArgument parametersArgument)
    {
        Guard.IsNotNull(parametersArgument);

        return parametersArgument.Values
            .Where(p => p?.Contains(':', StringComparison.CurrentCulture) == true)
            .Select(p => p!.Split(':'))
            .Select(p => new KeyValuePair<string, object?>(p[0], string.Join(":", p.Skip(1))))
            .ToArray();
    }

    protected static KeyValuePair<string, object?>[] MergeParameters(KeyValuePair<string, object?>[] parameters, KeyValuePair<string, object?>[] extractedTemplateParameters)
        => parameters
            .Concat(extractedTemplateParameters)
            .GroupBy(t => t.Key)
            .Select(x => new KeyValuePair<string, object?>(x.Key, x.First().Value))
            .ToArray();

    public abstract void Initialize(CommandLineApplication app);
}
