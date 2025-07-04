﻿namespace TemplateFramework.Console.Commands;

public class RunTemplateCommand : CommandBase
{
    private readonly ITemplateProvider _templateProvider;
    private readonly ITemplateEngine _templateEngine;

    public RunTemplateCommand(IClipboard clipboard, IFileSystem fileSystem, IUserInput userInput, ITemplateProvider templateProvider, ITemplateEngine templateEngine) : base(clipboard, fileSystem, userInput)
    {
        Guard.IsNotNull(templateProvider);
        Guard.IsNotNull(templateEngine);
        Guard.IsNotNull(userInput);

        _templateProvider = templateProvider;
        _templateEngine = templateEngine;
    }

    public override void Initialize(CommandLineApplication app)
    {
        Guard.IsNotNull(app);
        app.Command("template", command =>
        {
            command.Description = "Runs a template from a .NET class or a flat text file";

            var assemblyOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The assembly name", CommandOptionType.SingleValue);
            var classNameOption = command.Option<string>("-n|--classname <CLASSNAME>", "The template class name", CommandOptionType.SingleValue);
            var formattableStringTemplateFilenameOption = command.Option<string>("-f|--formattablestring <FILENAME>", "The file name of a formattable string template", CommandOptionType.SingleValue);
            var expressionStringTemplateFilenameOption = command.Option<string>("-e|--expressionstring <FILENAME>", "The file name of an expression string template", CommandOptionType.SingleValue);
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
            command.OnExecuteAsync(async cancellationToken =>
            {
                var assemblyName = assemblyOption.Value();
                var className = classNameOption.Value();
                var formattableStringTemplateFilename = formattableStringTemplateFilenameOption.Value();
                var expressionStringTemplateFilename = expressionStringTemplateFilenameOption.Value();

                var result = Validate(assemblyName, className, formattableStringTemplateFilename, expressionStringTemplateFilename);
                if (!result.IsSuccessful())
                {
                    await app.Error.WriteLineAsync(result.ErrorMessage).ConfigureAwait(false);
                    return;
                }

                await Execute((app,
                               watchOption.HasValue(),
                               interactiveOption.HasValue(),
                               listParametersOption.HasValue(),
                               bareOption.HasValue(),
                               clipboardOption.HasValue(),
                               assemblyName,
                               className,
                               formattableStringTemplateFilename,
                               expressionStringTemplateFilename,
                               currentDirectory: GetCurrentDirectory(currentDirectoryOption.Value(), assemblyName!),
                               templateProviderPluginClassName: templateProviderPluginClassNameOption.Value(),
                               basePath: GetBasePath(basePathOption.Value()),
                               defaultFilename: GetDefaultFilename(defaultFilenameOption.Value()),
                               dryRun: GetDryRun(dryRunOption.HasValue(), clipboardOption.HasValue()),
                               parameters: GetParameters(parametersArgument),
                               cancellationToken)).ConfigureAwait(false);
            });
        });
    }

    private Result Validate(string? assemblyName, string? className, string? formattableStringTemplateFilename, string? expressionStringTemplateFilename)
    {
        if (string.IsNullOrEmpty(formattableStringTemplateFilename) && string.IsNullOrEmpty(expressionStringTemplateFilename))
        {
            // Compiled template
            if (string.IsNullOrEmpty(assemblyName))
            {
                return Result.Error("Error: Assembly name is required.");
            }

            if (string.IsNullOrEmpty(className))
            {
                return Result.Error("Error: Class name is required.");
            }

            return Result.Success();
        }

        if (!string.IsNullOrEmpty(formattableStringTemplateFilename))
        {
            if (!FileSystem.FileExists(formattableStringTemplateFilename))
            {
                return Result.Error($"Error: File '{formattableStringTemplateFilename}' does not exist");
            }

            return Result.Success();
        }

        if (expressionStringTemplateFilename?.Length == 0)
        {
            return Result.Error("Error: Either AssemblyName and ClassName are required, or FormattableString template filename or ExpressionString template filename is required");
        }

        if (!FileSystem.FileExists(expressionStringTemplateFilename!))
        {
            return Result.Error($"Error: File '{expressionStringTemplateFilename}' does not exist");
        }

        return Result.Success();
    }

    private async Task Execute((CommandLineApplication app,
                                bool watch,
                                bool interactive,
                                bool listParameters,
                                bool bare,
                                bool clipboard,
                                string? assemblyName,
                                string? className,
                                string? formattableStringFilename,
                                string? expressionStringFilename,
                                string? currentDirectory,
                                string? templateProviderPluginClassName,
                                string basePath,
                                string defaultFilename,
                                bool dryRun,
                                KeyValuePair<string, object?>[] parameters,
                                CancellationToken cancellationToken) args)
        => await Watch(args.app, args.watch, args.assemblyName ?? args.formattableStringFilename ?? args.expressionStringFilename!, async () =>
        {
            var generationEnvironment = new MultipleStringContentBuilderEnvironment();
            var templateIdentifier = GetTemplateIdentifier(args);

            (await _templateProvider.StartSessionAsync(args.cancellationToken).ConfigureAwait(false))
            .OnFailure(async err => await args.app.Out.WriteLineAsync(err.ToString()).ConfigureAwait(false))
            .OnSuccess(async () =>
            {
                var template = _templateProvider.Create(templateIdentifier);
                var success = false;

                if (args.listParameters)
                {
                    if (string.IsNullOrEmpty(args.defaultFilename))
                    {
                        await args.app.Out.WriteLineAsync("Error: Default filename is required if you want to list parameters").ConfigureAwait(false);
                        return;
                    }

                    (await _templateEngine.GetParametersAsync(template, args.cancellationToken).ConfigureAwait(false))
                    .OnFailure(async err => await args.app.Out.WriteLineAsync(err.ToString()).ConfigureAwait(false))
                    .OnSuccess(
                        x =>
                        {
                            AppendParameters(generationEnvironment, args.defaultFilename, x.Value!);
                            success = true;
                        });
                }
                else
                {
                    if (args.interactive)
                    {
                        (await _templateEngine.GetParametersAsync(template, args.cancellationToken).ConfigureAwait(false))
                        .OnFailure(async err => await args.app.Out.WriteLineAsync(err.ToString()).ConfigureAwait(false))
                        .OnSuccess(
                            x =>
                            {
                                args.parameters = MergeParameters(args.parameters, GetInteractiveParameterValues(x.Value!));
                                success = true;
                            });
                    }

                    var context = new TemplateContext(_templateEngine, _templateProvider, args.defaultFilename, templateIdentifier, template);
                    var identifier = new TemplateInstanceIdentifierWithTemplateProvider(template, args.currentDirectory, args.assemblyName, args.templateProviderPluginClassName);
                    var request = new RenderTemplateRequest(identifier, null, generationEnvironment, args.defaultFilename, args.parameters, context);

                    (await _templateEngine.RenderAsync(request, args.cancellationToken).ConfigureAwait(false))
                    .OnFailure(async err => await args.app.Out.WriteLineAsync(err.ToString()).ConfigureAwait(false))
                    .OnSuccess(_ => success = true);
                }

                if (success)
                {
                    await WriteOutput(args.app, generationEnvironment, args.basePath, args.bare, args.clipboard, args.dryRun, args.cancellationToken).ConfigureAwait(false);
                }
            });
        }, args.cancellationToken).ConfigureAwait(false);

    private ITemplateIdentifier GetTemplateIdentifier((CommandLineApplication app, bool watch, bool interactive, bool listParameters, bool bare, bool clipboard, string? assemblyName, string? className, string? formattableStringFilename, string? expressionStringFilename, string? currentDirectory, string? templateProviderPluginClassName, string basePath, string defaultFilename, bool dryRun, KeyValuePair<string, object?>[] parameters, CancellationToken cancellationToken) args)
    {
        ITemplateIdentifier templateIdentifier = null!;
        if (!string.IsNullOrEmpty(args.className))
        {
            templateIdentifier = new CompiledTemplateIdentifier(args.assemblyName!, args.className!, args.currentDirectory);
        }
        else if (!string.IsNullOrEmpty(args.formattableStringFilename))
        {
            templateIdentifier = new FormattableStringTemplateIdentifier(FileSystem.ReadAllText(args.formattableStringFilename, Encoding.Default), CultureInfo.CurrentCulture, args.assemblyName, args.className, args.currentDirectory);
        }
        else if (!string.IsNullOrEmpty(args.expressionStringFilename))
        {
            templateIdentifier = new ExpressionStringTemplateIdentifier(FileSystem.ReadAllText(args.expressionStringFilename, Encoding.Default), CultureInfo.CurrentCulture, args.assemblyName, args.className, args.currentDirectory);
        }

        return templateIdentifier;
    }
}
