namespace TemplateFramework.Console.Commands;

public class RunTemplateCommand : CommandBase
{
    private readonly ITemplateProvider _templateProvider;
    private readonly ITemplateEngine _templateEngine;
    
    public RunTemplateCommand(ICodeGenerationAssembly codeGenerationAssembly, IClipboard clipboard, ITemplateProvider templateProvider, ITemplateEngine templateEngine) : base(codeGenerationAssembly, clipboard)
    {
        Guard.IsNotNull(templateProvider);
        Guard.IsNotNull(templateEngine);

        _templateProvider = templateProvider;
        _templateEngine = templateEngine;
    }

    public override void Initialize(CommandLineApplication app)
    {
        Guard.IsNotNull(app);
        app.Command("template", command =>
        {
            command.Description = "Runs a template";

            var assemblyOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The assembly name", CommandOptionType.SingleValue);
            var classNameOption = command.Option<string>("-n|--classname <CLASSNAME>", "The template class name", CommandOptionType.SingleValue);
            var watchOption = command.Option<string>("-w|--watch", "Watches for file changes", CommandOptionType.NoValue);
            var dryRunOption = command.Option<bool>("-r|--dryrun", "Indicator whether a dry run should be performed", CommandOptionType.NoValue);
            var basePathOption = command.Option<string>("-p|--path", "Base path for code generation", CommandOptionType.SingleValue);
            var defaultFilenameOption = command.Option<string>("-d|--default", "Default filename", CommandOptionType.SingleValue);
            var currentDirectoryOption = command.Option<string>("-i|--directory", "Current directory", CommandOptionType.SingleValue);
            var bareOption = command.Option<bool>("-b|--bare", "Bare output (only template output)", CommandOptionType.NoValue);
            var clipboardOption = command.Option<bool>("-c|--clipboard", "Copy output to clipboard", CommandOptionType.NoValue);
            command.HelpOption();
            command.OnExecute(() =>
            {
                var assemblyName = assemblyOption.Value();
                if (string.IsNullOrEmpty(assemblyName))
                {
                    app.Error.WriteLine("Error: Assembly name is required.");
                    return;
                }

                var className = classNameOption.Value();
                if (string.IsNullOrEmpty(assemblyName))
                {
                    app.Error.WriteLine("Error: Class name is required.");
                    return;
                }

                var currentDirectory = GetCurrentDirectory(currentDirectoryOption, assemblyName!);
                var basePath = GetBasePath(basePathOption);
                var defaultFilename = GetDefaultFilename(defaultFilenameOption);
                var dryRun = GetDryRun(dryRunOption, clipboardOption);

                Watch(app, watchOption, assemblyName, () =>
                {
                    var generationEnvironment = new MultipleContentBuilderEnvironment();
                    var createTemplateRequest = GetCreateTemplateRequest(assemblyName, className!, currentDirectory);

                    object additionalParameters = new(); //TODO: Add support for additional parameters in command line
                    var template = _templateProvider.Create(createTemplateRequest);
                    _templateEngine.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, additionalParameters, null));
                    WriteOutput(app, generationEnvironment, basePath, bareOption, clipboardOption, dryRun);
                });
            });
        });
    }

    private static CreateCompiledTemplateRequest GetCreateTemplateRequest(string assemblyName, string className, string? currentDirectory)
        => string.IsNullOrEmpty(currentDirectory)
            ? new CreateCompiledTemplateRequest(assemblyName, className!)
            : new CreateCompiledTemplateRequest(assemblyName, className!, currentDirectory);
}
