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
            var templateProviderPluginClassNameOption = command.Option<string>("-t|--templateproviderplugin <CLASSNAME>", "Optional class name for a template provider plug-in", CommandOptionType.SingleValue);
            var watchOption = command.Option<bool>("-w|--watch", "Watches for file changes", CommandOptionType.NoValue);
            var dryRunOption = command.Option<bool>("-r|--dryrun", "Indicator whether a dry run should be performed", CommandOptionType.NoValue);
            var basePathOption = command.Option<string>("-p|--path", "Base path for code generation", CommandOptionType.SingleValue);
            var defaultFilenameOption = command.Option<string>("-d|--default", "Default filename", CommandOptionType.SingleValue);
            var currentDirectoryOption = command.Option<string>("-i|--directory", "Current directory", CommandOptionType.SingleValue);
            var parametersArgument = command.Argument("Parameters", "Optional parameters to use (name:value)", true);
            var interactiveOption = command.Option<bool>("-i|--interactive", "Fill parameters interactively", CommandOptionType.NoValue);
            var listParametersOption = command.Option<bool>("-l|--list-parameters", "List parameters", CommandOptionType.NoValue);
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

                Execute((app,
                         watchOption.HasValue(),
                         interactiveOption.HasValue(),
                         listParametersOption.HasValue(),
                         bareOption.HasValue(),
                         clipboardOption.HasValue(),
                         assemblyName,
                         className,
                         currentDirectory: GetCurrentDirectory(currentDirectoryOption.Value(), assemblyName!),
                         templateProviderPluginClassName: templateProviderPluginClassNameOption.Value(),
                         basePath: GetBasePath(basePathOption.Value()),
                         defaultFilename: GetDefaultFilename(defaultFilenameOption.Value()),
                         dryRun: GetDryRun(dryRunOption.HasValue(), clipboardOption.HasValue()),
                         parameters: GetParameters(parametersArgument)));
            });
        });
    }

    private void Execute((CommandLineApplication app,
                         bool watch,
                         bool interactive,
                         bool listParameters,
                         bool bare,
                         bool clipboard,
                         string assemblyName,
                         string className,
                         string? currentDirectory,
                         string? templateProviderPluginClassName,
                         string basePath,
                         string defaultFilename,
                         bool dryRun,
                         KeyValuePair<string, object?>[] parameters) args)
    {
        Watch(args.app, args.watch, args.assemblyName, () =>
        {
            var generationEnvironment = new MultipleContentBuilderEnvironment();
            var createTemplateRequest = new CreateCompiledTemplateRequest(args.assemblyName, args.className, args.currentDirectory);

            var template = _templateProvider.Create(createTemplateRequest);

            if (args.listParameters)
            {
                if (string.IsNullOrEmpty(args.defaultFilename))
                {
                    args.app.Out.WriteLine("Error: Default filename is required if you want to list parameters");
                    return;
                }

                AppendParameters(generationEnvironment, args.defaultFilename, _templateEngine.GetParameters(template));
            }
            else
            {
                if (args.interactive)
                {
                    args.parameters = MergeParameters(args.parameters, GetInteractiveParameterValues(_templateEngine.GetParameters(template)));
                }

                var context = new TemplateContext(_templateEngine, _templateProvider, args.defaultFilename, createTemplateRequest, template);
                _templateEngine.Render(new RenderTemplateRequest(new TemplateInstanceIdentifierWithTemplateProvider(template, args.currentDirectory, args.assemblyName, args.templateProviderPluginClassName), null, generationEnvironment, args.defaultFilename, args.parameters, context));
            }

            WriteOutput(args.app, generationEnvironment, args.basePath, args.bare, args.clipboard, args.dryRun);
        });
    }

    private void AppendParameters(MultipleContentBuilderEnvironment generationEnvironment, string defaultFilename, ITemplateParameter[] templateParameters)
    {
        var content = generationEnvironment.Builder.AddContent(defaultFilename);
        foreach (var parameter in templateParameters)
        {
            content.Builder.AppendLine(CultureInfo.CurrentCulture, $"{parameter.Name} ({parameter.Type.FullName})");
        }
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

    private KeyValuePair<string, object?>[] MergeParameters(KeyValuePair<string, object?>[] parameters, KeyValuePair<string, object?>[] extractedTemplateParameters)
        => parameters
            .Concat(extractedTemplateParameters)
            .GroupBy(t => t.Key)
            .Select(x => new KeyValuePair<string, object?>(x.Key, x.First().Value))
            .ToArray();
}
