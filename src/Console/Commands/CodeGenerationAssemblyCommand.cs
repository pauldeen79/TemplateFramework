namespace TemplateFramework.Console.Commands;

public class CodeGenerationAssemblyCommand : ICommandLineCommand
{
    private readonly ICodeGenerationAssembly _codeGenerationAssembly;
    private readonly IClipboard _clipboard;

    public CodeGenerationAssemblyCommand(ICodeGenerationAssembly codeGenerationAssembly, IClipboard clipboard)
    {
        Guard.IsNotNull(codeGenerationAssembly);
        Guard.IsNotNull(clipboard);

        _codeGenerationAssembly = codeGenerationAssembly;
        _clipboard = clipboard;
    }

    public void Initialize(CommandLineApplication app)
    {
        Guard.IsNotNull(app);
        app.Command("assembly", command =>
        {
            command.Description = "Runs all code generation providers from the specified assembly";

            var assemblyOption = command.Option<string>("-a|--assembly <PATH>", "The assembly name", CommandOptionType.SingleValue);
            var watchOption = command.Option<string>("-w|--watch", "Watches for file changes", CommandOptionType.NoValue);
            var dryRunOption = command.Option<bool>("-r|--dryrun", "Indicator whether a dry run should be performed", CommandOptionType.NoValue);
            var basePathOption = command.Option<string>("-p|--path", "Base path for code generation", CommandOptionType.SingleValue);
            var defaultFilenameOption = command.Option<string>("-d|--default", "Default filename", CommandOptionType.SingleValue);
            var bareOption = command.Option<bool>("-b|--bare", "Bare output (only template output)", CommandOptionType.NoValue);
            var clipboardOption = command.Option<bool>("-c|--clipboard", "Copy output to clipboard", CommandOptionType.NoValue);
            var filterClassNameOption = command.Option<string>("-f|--filter <CLASSNAME>", "Filter code generation provider by class name", CommandOptionType.MultipleValue);
            command.HelpOption();
            command.OnExecute(() =>
            {
                var assemblyName = assemblyOption.Value();
                if (string.IsNullOrEmpty(assemblyName))
                {
                    app.Error.WriteLine("Error: Assembly name is required.");
                    return;
                }

                var currentDirectory = assemblyName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase)
                    ? Path.GetDirectoryName(assemblyName)
                    : string.Empty;

                var basePath = basePathOption.HasValue()
                    ? basePathOption.Value()!
                    : string.Empty;

                var defaultFilename = defaultFilenameOption.HasValue()
                    ? defaultFilenameOption.Value()!
                    : string.Empty;

                var dryRun = dryRunOption.HasValue() || clipboardOption.HasValue();
                
                CommandBase.Watch(app, watchOption, assemblyName, () =>
                {
                    var generationEnvironment = new MultipleContentBuilderEnvironment();
                    var classNameFilter = filterClassNameOption.Values.Where(x => x is not null).Select(x => x!);
                    var settings = new CodeGenerationAssemblySettings(basePath, defaultFilename, assemblyName, dryRun, currentDirectory, classNameFilter);
                    _codeGenerationAssembly.Generate(settings, generationEnvironment);
                    WriteOutput(app, generationEnvironment, basePath, bareOption, clipboardOption, dryRun);
                });
            });
        });
    }

    private void WriteOutput(CommandLineApplication app, MultipleContentBuilderEnvironment generationEnvironment, string basePath, CommandOption<bool> bareOption, CommandOption<bool> clipboardOption, bool dryRun)
    {
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

    private string GenerateSingleOutput(IMultipleContentBuilder builder, string basePath)
    {
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

    private void WriteOutputToClipboard(CommandLineApplication app, string templateOutput, CommandOption<bool> bareOption)
    {
        _clipboard.SetText(templateOutput);

        if (!bareOption.HasValue())
        {
            app.Out.WriteLine("Copied code generation output to clipboard");
        }
    }

    private static void WriteOutputToHost(CommandLineApplication app, string templateOutput, CommandOption<bool> bareOption)
    {
        if (!bareOption.HasValue())
        {
            app.Out.WriteLine("Code generation output:");
        }

        app.Out.WriteLine(templateOutput);
    }
}
