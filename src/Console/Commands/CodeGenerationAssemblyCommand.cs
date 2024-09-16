namespace TemplateFramework.Console.Commands;

public class CodeGenerationAssemblyCommand : CommandBase
{
    private readonly ICodeGenerationAssembly _codeGenerationAssembly;

    public CodeGenerationAssemblyCommand(IClipboard clipboard, IFileSystem fileSystem, IUserInput userInput, ICodeGenerationAssembly codeGenerationAssembly) : base(clipboard, fileSystem, userInput)
    {
        Guard.IsNotNull(codeGenerationAssembly);

        _codeGenerationAssembly = codeGenerationAssembly;
    }

    public override void Initialize(CommandLineApplication app)
    {
        Guard.IsNotNull(app);
        app.Command("assembly", command =>
        {
            command.Description = "Runs all code generation providers from the specified assembly";

            var assemblyOption = command.Option<string>("-n|--name <NAME>", "The assembly name", CommandOptionType.SingleValue);
            var watchOption = command.Option<string>("-w|--watch", "Watches for file changes", CommandOptionType.NoValue);
            var dryRunOption = command.Option<bool>("-r|--dryrun", "Indicator whether a dry run should be performed", CommandOptionType.NoValue);
            var basePathOption = command.Option<string>("-p|--path", "Base path for code generation", CommandOptionType.SingleValue);
            var defaultFilenameOption = command.Option<string>("-d|--default", "Default filename", CommandOptionType.SingleValue);
            var currentDirectoryOption = command.Option<string>("-i|--directory", "Current directory", CommandOptionType.SingleValue);
            var bareOption = command.Option<bool>("-b|--bare", "Bare output (only template output)", CommandOptionType.NoValue);
            var clipboardOption = command.Option<bool>("-c|--clipboard", "Copy output to clipboard", CommandOptionType.NoValue);
            var filterClassNameOption = command.Option<string>("-f|--filter <CLASSNAME>", "Filter code generation provider by class name", CommandOptionType.MultipleValue);
            command.HelpOption();
            command.OnExecuteAsync(async cancellationToken =>
            {
                var assemblyName = assemblyOption.Value();
                if (string.IsNullOrEmpty(assemblyName))
                {
                    await app.Error.WriteLineAsync("Error: Assembly name is required.").ConfigureAwait(false);
                    return;
                }

                var basePath = GetBasePath(basePathOption.Value());
                var dryRun = GetDryRun(dryRunOption.HasValue(), clipboardOption.HasValue());

                await Watch(app, watchOption.HasValue(), assemblyName, async () =>
                {
                    var generationEnvironment = new MultipleStringContentBuilderEnvironment();
                    var classNameFilter = filterClassNameOption.Values.Where(x => x is not null).Select(x => x!);
                    var settings = new CodeGenerationAssemblySettings(basePath, GetDefaultFilename(defaultFilenameOption.Value()), assemblyName, dryRun, GetCurrentDirectory(currentDirectoryOption.Value(), assemblyName!), classNameFilter);
                    await _codeGenerationAssembly.Generate(settings, generationEnvironment, cancellationToken).ConfigureAwait(false);
                    await WriteOutput(app, generationEnvironment, basePath, bareOption.HasValue(), clipboardOption.HasValue(), dryRun, cancellationToken).ConfigureAwait(false);
                }, cancellationToken).ConfigureAwait(false);
            });
        });
    }
}
