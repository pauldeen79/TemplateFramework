namespace TemplateFramework.Console.Commands;

public class CodeGenerationAssemblyCommand : CommandBase
{
    private readonly ICodeGenerationAssembly _codeGenerationAssembly;

    public CodeGenerationAssemblyCommand(ICodeGenerationAssembly codeGenerationAssembly, IClipboard clipboard, IFileSystem fileSystem) : base(clipboard, fileSystem)
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
            command.OnExecute(() =>
            {
                var assemblyName = assemblyOption.Value();
                if (string.IsNullOrEmpty(assemblyName))
                {
                    app.Error.WriteLine("Error: Assembly name is required.");
                    return;
                }

                var currentDirectory = GetCurrentDirectory(currentDirectoryOption.Value(), assemblyName!);
                var basePath = GetBasePath(basePathOption);
                var defaultFilename = GetDefaultFilename(defaultFilenameOption);
                var dryRun = GetDryRun(dryRunOption, clipboardOption);

                Watch(app, watchOption, assemblyName, () =>
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
}
