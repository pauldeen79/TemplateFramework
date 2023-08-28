namespace TemplateFramework.Console.Commands;

public class RunTemplateCommand : CommandBase
{
    private readonly ITemplateProvider _templateProvider;
    private readonly ITemplateEngine _templateEngine;
    private readonly IUserInput _userInput;
    
    public RunTemplateCommand(IClipboard clipboard, ITemplateProvider templateProvider, ITemplateEngine templateEngine, IFileSystem fileSystem, IUserInput userInput) : base(clipboard, fileSystem)
    {
        Guard.IsNotNull(templateProvider);
        Guard.IsNotNull(templateEngine);
        Guard.IsNotNull(userInput);

        _templateProvider = templateProvider;
        _templateEngine = templateEngine;
        _userInput = userInput;
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
            var parametersArgument = command.Argument("Parameters", "Optional parameters to use (name:value)", true);
            var interactiveOption = command.Option<string>("-i|--interactive", "Fill parameters interactively", CommandOptionType.NoValue);
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
                if (string.IsNullOrEmpty(className))
                {
                    app.Error.WriteLine("Error: Class name is required.");
                    return;
                }

                var currentDirectory = GetCurrentDirectory(currentDirectoryOption.Value(), assemblyName!);
                var basePath = GetBasePath(basePathOption.Value());
                var defaultFilename = GetDefaultFilename(defaultFilenameOption.Value());
                var dryRun = GetDryRun(dryRunOption.HasValue(), clipboardOption.HasValue());
                var parameters = GetParameters(parametersArgument);

                Watch(app, watchOption.HasValue(), assemblyName, () =>
                {
                    var generationEnvironment = new MultipleContentBuilderEnvironment();
                    var createTemplateRequest = GetCreateTemplateRequest(assemblyName, className!, currentDirectory);

                    var template = _templateProvider.Create(createTemplateRequest);
                    if (interactiveOption.HasValue())
                    {
                        parameters = MergeParameters(parameters, GetInteractiveParameterValues(_templateEngine.GetParameters(template)));
                    }

                    _templateEngine.Render(new RenderTemplateRequest(template, null, generationEnvironment, defaultFilename, parameters, null));
                    WriteOutput(app, generationEnvironment, basePath, bareOption.HasValue(), clipboardOption.HasValue(), dryRun);
                });
            });
        });
    }

    private static KeyValuePair<string, object?>[] GetParameters(CommandArgument parametersArgument)
        => parametersArgument.Values
            .Where(p => p?.Contains(':', StringComparison.CurrentCulture) == true)
            .Select(p => p!.Split(':'))
            .Select(p => new KeyValuePair<string, object?>(p[0], string.Join(":", p.Skip(1))))
            .ToArray();

    private KeyValuePair<string, object?>[] GetInteractiveParameterValues(ITemplateParameter[] templateParameters)
    {
        var list = new List<KeyValuePair<string, object?>>();
        foreach (var templateParameter in templateParameters)
        {
            var value = _userInput.GetValue(templateParameter);
            list.Add(new KeyValuePair<string, object?>(templateParameter.Name, value));
        }

        return list.ToArray();
    }

    private static CreateCompiledTemplateRequest GetCreateTemplateRequest(string assemblyName, string className, string? currentDirectory)
        => string.IsNullOrEmpty(currentDirectory)
            ? new CreateCompiledTemplateRequest(assemblyName, className!)
            : new CreateCompiledTemplateRequest(assemblyName, className!, currentDirectory);

    private KeyValuePair<string, object?>[] MergeParameters(KeyValuePair<string, object?>[] parameters, KeyValuePair<string, object?>[] extractedTemplateParameters)
        => parameters
            .Concat(extractedTemplateParameters)
            .GroupBy(t => t.Key)
            .Select(x => new KeyValuePair<string, object?>(x.Key, x.First().Value))
            .ToArray();
}
