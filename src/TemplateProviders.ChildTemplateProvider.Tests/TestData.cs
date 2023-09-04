namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

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

    internal sealed class MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine : IMultipleContentBuilderTemplate, ITemplateContextContainer
    {
        private readonly Action<IMultipleContentBuilder, ITemplateContext> _delegate;

        public MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(Action<IMultipleContentBuilder, ITemplateContext> @delegate)
        {
            _delegate = @delegate;
        }

        public ITemplateContext Context { get; set; } = default!;

        public void Render(IMultipleContentBuilder builder) => _delegate(builder, Context);
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
    internal sealed class CsharpClassGenerator : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
    {
        public void Render(IMultipleContentBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);
            Guard.IsNotNull(Context);

            StringBuilder? singleStringBuilder = null;
            IGenerationEnvironment generationEnvironment = new MultipleContentBuilderEnvironment(builder);

            if (!Model.Settings.GenerateMultipleFiles)
            {
                // Generate a single generation environment, so we create only a single file in the multiple content builder environment.
                singleStringBuilder = builder.AddContent(Context.DefaultFilename, Model.Settings.SkipWhenFileExists).Builder;
                generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
                RenderHeader(generationEnvironment);
            }

            RenderNamespaceHierarchy(generationEnvironment, singleStringBuilder);
        }

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            if (Model.Settings.GenerateMultipleFiles)
            {
                throw new NotSupportedException("Can't generate multiple files, because the generation environment has a single StringBuilder instance");
            }

            var generationEnvironment = new StringBuilderEnvironment(builder);
            RenderHeader(generationEnvironment);
            RenderNamespaceHierarchy(generationEnvironment, builder);
        }

        private void RenderHeader(IGenerationEnvironment generationEnvironment)
        {
            Context.Engine.RenderChildTemplate(
                Model!.Settings,
                generationEnvironment,
                Context,
                new TemplateByNameIdentifier("CodeGenerationHeader"));

            if (Context.IsRootContext)
            {
                Context.Engine.RenderChildTemplate(
                    Model,
                    generationEnvironment,
                    Context,
                    new TemplateByNameIdentifier("DefaultUsings")
                    );
            }
        }

        private void RenderNamespaceHierarchy(IGenerationEnvironment generationEnvironment, StringBuilder? singleStringBuilder)
        {
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

                Context.Engine.RenderChildTemplates(
                    typeBaseItems,
                    generationEnvironment,
                    Context,
                    typeBase => new TemplateByModelIdentifier(((CsharpClassGeneratorViewModel<TypeBase>)typeBase!).Data)
                    );

                if (Context.IsRootContext && singleStringBuilder is not null)
                {
                    singleStringBuilder.AppendLine("}"); // close namespace
                }
            }
        }
    }

    public sealed class CodeGenerationHeaderTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorSettings>, IStringBuilderTemplate
    {
        public string Version
            => !string.IsNullOrEmpty(Model?.EnvironmentVersion)
                ? Model.EnvironmentVersion
                : Environment.Version.ToString();

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            if (!Model.CreateCodeGenerationHeader)
            {
                return;
            }

            builder.AppendLine(Model.CultureInfo, $$"""
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: {{ Version }}
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
""");
        }
    }

    public sealed class DefaultUsingsTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IStringBuilderTemplate
    {
        public void Render(StringBuilder builder)
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
        }

        private readonly static string[] DefaultUsings = new[]
{
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text"
        };

        public IEnumerable<string> Usings
            => DefaultUsings
                .Union(Model?.Data.SelectMany(classItem => classItem.Usings) ?? Enumerable.Empty<string>())
                .OrderBy(ns => ns)
                .Distinct();
    }

    public sealed class ClassTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<TypeBase>>, IMultipleContentBuilderTemplate
    {
        public void Render(IMultipleContentBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);
            Guard.IsNotNull(Context);

            StringBuilderEnvironment generationEnvironment;

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
                Context.Engine.RenderChildTemplate(
                    Model.Settings,
                    generationEnvironment,
                    Context,
                    new TemplateByNameIdentifier("CodeGenerationHeader")
                    );
                Context.Engine.RenderChildTemplate(
                    new CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>(new[] { Model.Data }, Model.Settings),
                    generationEnvironment,
                    Context,
                    new TemplateByNameIdentifier("DefaultUsings")
                    );
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
            if (Model.Data.SubClasses?.Any() == true)
            {
                Context.Engine.RenderChildTemplates(
                    Model.Data.SubClasses.Select(typeBase => new CsharpClassGeneratorViewModel<TypeBase>(typeBase, Model.Settings.ForSubclasses())),
                    new MultipleContentBuilderEnvironment(builder),
                    Context,
                    model => new TemplateByModelIdentifier(model!.GetType().GetProperty(nameof(CsharpClassGeneratorViewModel<TypeBase>.Data))!.GetValue(model))
                    );
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

public sealed class CsharpClassGeneratorCodeGenerationProvider : ICodeGenerationProvider, ITemplateProviderPlugin
{
    public string Path => string.Empty;
    public bool RecurseOnDeleteGeneratedFiles => false;
    public string LastGeneratedFilesFilename => string.Empty;
    public Encoding Encoding => Encoding.UTF8;

    public object? CreateAdditionalParameters() => null;

    public Type GetGeneratorType() => typeof(TestData.CsharpClassGenerator);

    public object? CreateModel()
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
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1a", Usings = new[] { "ModelFramework" } },
            new TestData.TypeBase { Namespace  = "Namespace1", Name = "Class1b", Usings = new[] { "ModelFramework", "ModelFramework.Domain" } },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2a" },
            new TestData.TypeBase { Namespace  = "Namespace2", Name = "Class2b", SubClasses = new[] { new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass1" }, new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass2", SubClasses = new[] { new TestData.TypeBase { Namespace = "Ignored", Name = "Subclass2a" } } } } },
        };

        var viewModel = new TestData.CsharpClassGeneratorViewModel<IEnumerable<TestData.TypeBase>>(model, settings);

        return viewModel;
    }

    public void Initialize(ITemplateProvider provider)
    {
        Guard.IsNotNull(provider);

        var registrations = new List<ITemplateCreator>
        {
            new TemplateCreator<TestData.CodeGenerationHeaderTemplate>("CodeGenerationHeader"),
            new TemplateCreator<TestData.DefaultUsingsTemplate>("DefaultUsings"),
            new TemplateCreator<TestData.ClassTemplate>(typeof(TestData.TypeBase))
        };

        provider.RegisterComponent(new ProviderComponent(registrations));
    }
}
