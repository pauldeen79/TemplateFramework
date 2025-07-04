﻿namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

internal static class TestData
{
#if Windows
    internal const string BasePath = @"C:\Somewhere";
#elif Linux
    internal const string BasePath = @"/usr/bin/python3";
#elif OSX
    internal const string BasePath = @"/Users/moi/Downloads";
#else
    internal const string BasePath = "Unknown basepath, only Windows, Linux and OSX are supported";
#endif

    internal sealed class PlainTemplateWithTemplateContext : ITemplateContextContainer
    {
        private readonly Func<ITemplateContext, string> _delegate;

        public PlainTemplateWithTemplateContext(Func<ITemplateContext, string> @delegate) => _delegate = @delegate;

        public ITemplateContext Context { get; set; } = default!;
        public override string ToString() => _delegate(Context);
    }

    internal sealed class TemplateWithViewModel<T> : IBuilderTemplate<StringBuilder>, IModelContainer<T>
    {
        public T? Model { get; set; }

        private readonly Action<StringBuilder, T> _delegate;

        public TemplateWithViewModel(Action<StringBuilder, T> @delegate) => _delegate = @delegate;

        public Task<Result> RenderAsync(StringBuilder builder, CancellationToken cancellationToken) { _delegate(builder, Model!); return Task.FromResult(Result.Success()); }
    }

    // False positive, it gets created through DI container
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    internal sealed class ViewModel<TModel> : IModelContainer<TModel>, IViewModel
    {
        public TModel? Model { get; set; }
    }
#pragma warning restore CA1812 // Avoid uninstantiated internal classes

    internal sealed class Model
    {
        public string? Contents { get; set; }
    }

    internal sealed class MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine : IMultipleContentBuilderTemplate, ITemplateContextContainer
    {
        private readonly Func<IMultipleContentBuilder<StringBuilder>, ITemplateContext, Task> _delegate;

        public MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(Func<IMultipleContentBuilder<StringBuilder>, ITemplateContext, Task> @delegate)
        {
            _delegate = @delegate;
        }

        public ITemplateContext Context { get; set; } = default!;

        public async Task<Result> RenderAsync(IMultipleContentBuilder<StringBuilder> builder, CancellationToken cancellationToken)
        {
            await _delegate(builder, Context).ConfigureAwait(false);
            return Result.Success();
        }
    }

    internal sealed class CsharpClassGeneratorViewModel<TModel>
    {
        public CsharpClassGeneratorViewModel(TModel data, CsharpClassGeneratorSettings settings)
        {
            Data = data;
            Settings = settings;
        }

        public TModel Data { get; }
        public CsharpClassGeneratorSettings Settings { get; }
    }

    internal sealed record CsharpClassGeneratorSettings
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public CsharpClassGeneratorSettings(bool generateMultipleFiles,
                                            bool skipWhenFileExists,
                                            bool createCodeGenerationHeader,
                                            string? environmentVersion,
                                            string? filenamePrefix,
                                            string? filenameSuffix,
                                            bool enableNullableContext,
                                            int indentCount,
                                            CultureInfo cultureInfo)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            GenerateMultipleFiles = generateMultipleFiles;
            SkipWhenFileExists = skipWhenFileExists;
            CreateCodeGenerationHeader = createCodeGenerationHeader;
            EnvironmentVersion = environmentVersion;
            FilenamePrefix = filenamePrefix;
            FilenameSuffix = filenameSuffix;
            EnableNullableContext = enableNullableContext;
            IndentCount = indentCount;
            CultureInfo = cultureInfo;
        }

        public bool GenerateMultipleFiles { get; }
        public bool SkipWhenFileExists { get; }
        public bool CreateCodeGenerationHeader { get; }
        public string? EnvironmentVersion { get; }
        public string? FilenamePrefix { get; }
        public string? FilenameSuffix { get; }
        public bool EnableNullableContext { get; }
        public int IndentCount { get; }
        public CultureInfo CultureInfo { get; }

        public CsharpClassGeneratorSettings ForSubclasses()
            => new(false, SkipWhenFileExists, false, null, null, null, false, IndentCount + 1, CultureInfo);
    }

    internal abstract class CsharpClassGeneratorBase<TModel> : IModelContainer<TModel>, ITemplateContextContainer
    {
        public ITemplateContext Context { get; set; } = default!;
        public TModel? Model { get; set; }
    }

    // False positive, it gets created through DI container
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    internal sealed class CsharpClassGenerator : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IMultipleContentBuilderTemplate, IBuilderTemplate<StringBuilder>
    {
        public async Task<Result> RenderAsync(IMultipleContentBuilder<StringBuilder> builder, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);
            Guard.IsNotNull(Context);

            StringBuilder? singleStringBuilder = null;
            IGenerationEnvironment generationEnvironment = new MultipleStringContentBuilderEnvironment(builder);

            if (!Model.Settings.GenerateMultipleFiles)
            {
                // Generate a single generation environment, so we create only a single file in the multiple content builder environment.
                singleStringBuilder = builder.AddContent(Context.DefaultFilename, Model.Settings.SkipWhenFileExists).Builder;
                generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
                var result = await RenderHeader(generationEnvironment, cancellationToken).ConfigureAwait(false);
                if (!result.IsSuccessful())
                {
                    return result;
                }
            }

            return await RenderNamespaceHierarchy(generationEnvironment, singleStringBuilder, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> RenderAsync(StringBuilder builder, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            if (Model.Settings.GenerateMultipleFiles)
            {
                throw new NotSupportedException("Can't generate multiple files, because the generation environment has a single StringBuilder instance");
            }

            var generationEnvironment = new StringBuilderEnvironment(builder);
            var result = await RenderHeader(generationEnvironment, cancellationToken).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
            return await RenderNamespaceHierarchy(generationEnvironment, builder, cancellationToken).ConfigureAwait(false);
        }

        private async Task<Result> RenderHeader(IGenerationEnvironment generationEnvironment, CancellationToken cancellationToken)
        {
            var result = await Context.Engine.RenderChildTemplateAsync(
                Model!.Settings,
                generationEnvironment,
                Context,
                new TemplateByNameIdentifier("CodeGenerationHeader"),
                cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccessful())
            {
                return result;
            }

            if (Context.IsRootContext)
            {
                result = await Context.Engine.RenderChildTemplateAsync(
                    Model,
                    generationEnvironment,
                    Context,
                    new TemplateByNameIdentifier("DefaultUsings"),
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<Result> RenderNamespaceHierarchy(IGenerationEnvironment generationEnvironment, StringBuilder? singleStringBuilder, CancellationToken cancellationToken)
        {
            var result = Result.Success();
            foreach (var ns in Model!.Data.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                if (Context.IsRootContext && singleStringBuilder is not null)
                {
                    singleStringBuilder.AppendLine(Model.Settings.CultureInfo, $"namespace {ns.Key}");
                    singleStringBuilder.AppendLine("{"); // open namespace
                }

                var typeBaseItems = ns
                    .OrderBy(typeBase => typeBase.Name)
                    .Select(typeBase => new CsharpClassGeneratorViewModel<TypeBase>(typeBase, Model.Settings));

                result = await Context.Engine.RenderChildTemplatesAsync(
                    typeBaseItems,
                    generationEnvironment,
                    Context,
                    typeBase => new TemplateByModelIdentifier(((CsharpClassGeneratorViewModel<TypeBase>)typeBase!).Data),
                    cancellationToken
                    ).ConfigureAwait(false);
                if (!result.IsSuccessful())
                {
                    return result;
                }

                if (Context.IsRootContext && singleStringBuilder is not null)
                {
                    singleStringBuilder.AppendLine("}"); // close namespace
                }

            }

            return result;
        }
    }

    public sealed class CodeGenerationHeaderTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorSettings>, IBuilderTemplate<StringBuilder>
    {
        public string Version
            => !string.IsNullOrEmpty(Model?.EnvironmentVersion)
                ? Model.EnvironmentVersion
                : Environment.Version.ToString();

        public Task<Result> RenderAsync(StringBuilder builder, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            if (!Model.CreateCodeGenerationHeader)
            {
                return Task.FromResult(Result.NoContent());
            }

            builder.AppendLine(Model.CultureInfo, $$"""
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: {{Version}}
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
""");

            return Task.FromResult(Result.Success());
        }
    }

    public sealed class DefaultUsingsTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IBuilderTemplate<StringBuilder>
    {
        public Task<Result> RenderAsync(StringBuilder builder, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            var anyUsings = false;
            foreach (var @using in Usings)
            {
                builder.AppendLine(Model.Settings.CultureInfo, $"using {@using};");
                anyUsings = true;
            }

            if (anyUsings)
            {
                builder.AppendLine();
            }

            return Task.FromResult(Result.Success());
        }

        private static readonly string[] DefaultUsings =
        [
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text"
        ];

        public IEnumerable<string> Usings
            => DefaultUsings
                .Union(Model?.Data.SelectMany(classItem => classItem.Usings) ?? Enumerable.Empty<string>())
                .OrderBy(ns => ns)
                .Distinct();
    }

    public sealed class ClassTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<TypeBase>>, IMultipleContentBuilderTemplate
    {
        public async Task<Result> RenderAsync(IMultipleContentBuilder<StringBuilder> builder, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);
            Guard.IsNotNull(Context);

            StringBuilderEnvironment generationEnvironment;
            Result result = Result.Success();

            if (!Model.Settings.GenerateMultipleFiles)
            {
                if (!builder.Contents.Any())
                {
                    builder.AddContent(Context.DefaultFilename, Model.Settings.SkipWhenFileExists);
                }

                generationEnvironment = new StringBuilderEnvironment(builder.Contents.Last().Builder); // important to take the last contents, in case of sub classes
            }
            else
            {
                var filename = $"{Model.Settings.FilenamePrefix}{Model.Data.Name}{Model.Settings.FilenameSuffix}.cs";
                var contentBuilder = builder.AddContent(filename, Model.Settings.SkipWhenFileExists);
                generationEnvironment = new StringBuilderEnvironment(contentBuilder.Builder);
                var actions = new[]
                {
                    Context.Engine.RenderChildTemplateAsync(
                        Model.Settings,
                        generationEnvironment,
                        Context,
                        new TemplateByNameIdentifier("CodeGenerationHeader"),
                        cancellationToken
                        ),
                    Context.Engine.RenderChildTemplateAsync(
                        new CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>([Model.Data], Model.Settings),
                        generationEnvironment,
                        Context,
                        new TemplateByNameIdentifier("DefaultUsings"),
                        cancellationToken
                        )
                };
                var errors = Array.FindAll(await Task.WhenAll(actions).ConfigureAwait(false), x => !x.IsSuccessful());
                result = Result.Aggregate(errors, Result.Success(), nonSuccesfulResults => Result.Error(nonSuccesfulResults, "One or more child templates returned a non-succesful result, see the inner results for more details"));
                if (!result.IsSuccessful())
                {
                    return result;
                }
                contentBuilder.Builder.AppendLine(Model.Settings.CultureInfo, $"namespace {Model.Data.Namespace}");
                contentBuilder.Builder.AppendLine("{"); // start namespace
            }

            if (Model.Settings.EnableNullableContext && Model.Settings.IndentCount == 1)
            {
                generationEnvironment.Builder.AppendLine("#nullable enable");
            }

            var indentedBuilder = new IndentedStringBuilder(generationEnvironment.Builder);
            for (int i = 0; i < Model.Settings.IndentCount; i++)
            {
                indentedBuilder.Indent();
            }
            //TODO: Render attributes
            //TODO: Replace public class with more options based on model
            indentedBuilder.AppendLine($"public class {Model.Data.Name}");
            indentedBuilder.AppendLine("{"); // start class

            //TODO: Render child items
            if (Model.Data.SubClasses is not null && Model.Data.SubClasses.Length > 0)
            {
                result = await Context.Engine.RenderChildTemplatesAsync(
                    Model.Data.SubClasses.Select(typeBase => new CsharpClassGeneratorViewModel<TypeBase>(typeBase, Model.Settings.ForSubclasses())),
                    new MultipleStringContentBuilderEnvironment(builder),
                    Context,
                    model => new TemplateByModelIdentifier(model!.GetType().GetProperty(nameof(CsharpClassGeneratorViewModel<TypeBase>.Data))!.GetValue(model)),
                    cancellationToken
                    ).ConfigureAwait(false);

                if (!result.IsSuccessful())
                {
                    return result;
                }
            }

            indentedBuilder.AppendLine("}"); // end class

            if (Model.Settings.EnableNullableContext && Model.Settings.IndentCount == 1)
            {
                generationEnvironment.Builder.AppendLine("#nullable restore");
            }

            if (Model.Settings.GenerateMultipleFiles)
            {
                generationEnvironment.Builder.AppendLine("}"); // end namespace
            }

            return result;
        }
    }
#pragma warning restore CA1812 // Avoid uninstantiated internal classes

    internal sealed class TypeBase
    {
        public string Namespace { get; set; } = "";
        public string Name { get; set; } = "";
        public TypeBase[]? SubClasses { get; set; }
        public string[] Usings { get; set; } = Array.Empty<string>();
    }
}

public sealed class CsharpClassGeneratorCodeGenerationProvider : ICodeGenerationProvider, ITemplateComponentRegistryPlugin
{
    public string Path => string.Empty;
    public bool RecurseOnDeleteGeneratedFiles => false;
    public string LastGeneratedFilesFilename => string.Empty;
    public Encoding Encoding => Encoding.UTF8;

    public Task<Result<object?>> CreateAdditionalParametersAsync(CancellationToken cancellationToken) => Task.FromResult(Result.Success<object?>(default));

    public Type GetGeneratorType() => typeof(TestData.CsharpClassGenerator);

    public Task<Result<object?>> CreateModelAsync(CancellationToken cancellationToken)
    {
        var settings = new TestData.CsharpClassGeneratorSettings
        (
            generateMultipleFiles: true,
            skipWhenFileExists: false,
            createCodeGenerationHeader: true,
            environmentVersion: "1.0",
            filenamePrefix: "Entities/",
            filenameSuffix: ".generated",
            enableNullableContext: true,
            indentCount: 1,
            cultureInfo: CultureInfo.CurrentCulture
        );
        var model = new[]
        {
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1a", Usings = ["ModelFramework"] },
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1b", Usings = ["ModelFramework", "ModelFramework.Domain"] },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2a" },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2b", SubClasses = [new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass1" }, new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass2", SubClasses = [new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass2a" }] }] },
        };

        var viewModel = new TestData.CsharpClassGeneratorViewModel<IEnumerable<TestData.TypeBase>>(model, settings);

        return Task.FromResult(Result.Success<object?>(viewModel));
    }

    public Task<Result> InitializeAsync(ITemplateComponentRegistry registry, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(registry);

        var registrations = new List<ITemplateCreator>
        {
            new TemplateCreator<TestData.CodeGenerationHeaderTemplate>("CodeGenerationHeader"),
            new TemplateCreator<TestData.DefaultUsingsTemplate>("DefaultUsings"),
            new TemplateCreator<TestData.ClassTemplate>(typeof(TestData.TypeBase))
        };

        registry.RegisterComponent(new ProviderComponent(registrations));
        return Task.FromResult(Result.Success());
    }
}

public class XDocumentTemplate : IBuilderTemplate<XDocumentBuilder>, ITemplateContextContainer, IModelContainer<XDocumentTestModel>
{
    public ITemplateContext Context { get; set; } = default!;
    public XDocumentTestModel? Model { get; set; }

    public async Task<Result> RenderAsync(XDocumentBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Context);
        Guard.IsNotNull(Model);

        builder.CurrentElement.Add(new XAttribute("processed", true));

        // POC to add an XElement with a collection.
        // Note that currently, you have to update the current element, so the child template knows where content should be added to.
        builder.CurrentElement.Add(new XElement("subItems"));
        builder.CurrentElement = builder.CurrentElement.Element("subItems")!;
        // Because this is just a POC, we are using a collection of strings, and a named template.
        // If you are using a (View)Model, then you can omit the name and resolve the template by model type.
        return await Context.Engine.RenderChildTemplatesAsync(Model.SubItems, new XDocumentGenerationEnvironment(builder), new TemplateByNameIdentifier("SubItem"), Context, cancellationToken).ConfigureAwait(false);
    }
}

public class SubItemTemplate : IBuilderTemplate<XDocumentBuilder>, IModelContainer<string>
{
    public string? Model { get; set; }

    public Task<Result> RenderAsync(XDocumentBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.CurrentElement.Add(new XElement("item", Model));

        return Task.FromResult(Result.Success());
    }
}

public class XDocumentTestModel
{
    public XDocumentTestModel(params string[] subItems)
    {
        SubItems = subItems;
    }

    public IReadOnlyCollection<string> SubItems { get; }
}

public class XDocumentBuilder
{
    public XDocumentBuilder(XElement rootElement)
    {
        Guard.IsNotNull(rootElement);

        Document = new XDocument(rootElement);
        CurrentElement = Document.Root!;
    }

    public XDocument Document { get; }
    public XElement CurrentElement { get; set; }
}

public class XDocumentGenerationEnvironment : GenerationEnvironmentBase<XDocumentBuilder>
{
    public XDocumentGenerationEnvironment(XElement rootElement)
        : base(new XDocumentBuilder(rootElement))
    {
    }

    public XDocumentGenerationEnvironment(XDocumentBuilder builder)
        : base(builder)
    {
    }

    internal XDocumentGenerationEnvironment(IFileSystem fileSystem, IRetryMechanism retryMechanism, XDocumentBuilder builder)
        : base(fileSystem, retryMechanism, builder)
    {
    }

    protected override string Build() => Builder.Document.ToString();
}

public class XDocumentBuilderTemplateRenderer : BuilderTemplateRendererBase<XDocumentGenerationEnvironment, XDocumentBuilder>
{
    public XDocumentBuilderTemplateRenderer(IEnumerable<IBuilderTemplateRenderer<XDocumentBuilder>> renderers) : base(renderers)
    {
    }

    protected override Task<Result> DefaultImplementationAsync(object templateInstance, XDocumentBuilder builder)
        => Task.FromResult(Result.NotSupported($"Type of Template ({templateInstance.GetType().FullName}) is not supported"));
}
