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

    internal abstract class CsharpClassGeneratorBase<TModel> : IModelContainer<TModel>, IDefaultFilenameContainer
    {
        protected ITemplateContext Context => new TemplateContext(Engine, Provider, DefaultFilename, this, Model);
        protected ITemplateEngine Engine { get; }
        protected ITemplateProvider Provider { get; }

        public string DefaultFilename { get; set; } = "";
        public TModel? Model { get; set; }

        protected CsharpClassGeneratorBase(ITemplateEngine engine, ITemplateProvider provider)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
    }

    internal sealed class CsharpClassGenerator : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IMultipleContentBuilderTemplate
    {
        public CsharpClassGenerator(ITemplateEngine engine, ITemplateProvider provider) : base(engine, provider)
        {
        }

        public void Render(IMultipleContentBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            StringBuilder? singleStringBuilder = null;
            IGenerationEnvironment generationEnvironment = new MultipleContentBuilderEnvironment(builder);

            if (!Model.Settings.GenerateMultipleFiles)
            {
                singleStringBuilder = builder.AddContent(Context.DefaultFilename, Model.Settings.SkipWhenFileExists).Builder;
                generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
                Context.Engine.RenderChildTemplate(
                    Model.Settings,
                    generationEnvironment,
                    Context,
                    new CreateChildTemplateByNameRequest("CodeGenerationHeader"));

                if (Context.IsRootContext)
                {
                    Context.Engine.RenderChildTemplate(
                        Model,
                        generationEnvironment,
                        Context,
                        new CreateChildTemplateByNameRequest("DefaultUsings")
                        );
                }
            }

            foreach (var ns in Model.Data.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                if (Context.IsRootContext && !Model.Settings.GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine(Model.Settings.CultureInfo, $"namespace {ns.Key}");
                    singleStringBuilder.AppendLine("{"); // open namespace
                }

                var typeBaseItems = ns
                    .OrderBy(typeBase => typeBase.Name)
                    .Select(typeBase => new CsharpClassGeneratorViewModel<TypeBase>(typeBase, Model.Settings));

                Context.Engine.RenderChildTemplates(
                    typeBaseItems,
                    generationEnvironment,
                    Context,
                    typeBase => new CreateChildTemplateByModelRequest(((CsharpClassGeneratorViewModel<TypeBase>)typeBase!).Data)
                    );

                if (Context.IsRootContext && !Model.Settings.GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine("}"); // close namespace
                }
            }
        }
    }

    internal sealed class CodeGenerationHeaderTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorSettings>, IStringBuilderTemplate
    {
        public CodeGenerationHeaderTemplate(ITemplateEngine engine, ITemplateProvider provider) : base(engine, provider)
        {
        }

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

    internal sealed class DefaultUsingsTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IStringBuilderTemplate
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

        public DefaultUsingsTemplate(ITemplateEngine engine, ITemplateProvider provider) : base(engine, provider)
        {
        }

        public IEnumerable<string> Usings
            => DefaultUsings
                .Union(Model?.Data.SelectMany(classItem => classItem.Usings) ?? Enumerable.Empty<string>())
                .OrderBy(ns => ns)
                .Distinct();
    }

    internal sealed class ClassTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<TypeBase>>, IMultipleContentBuilderTemplate
    {
        public ClassTemplate(ITemplateEngine engine, ITemplateProvider provider) : base(engine, provider)
        {
        }

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
                    new CreateChildTemplateByNameRequest("CodeGenerationHeader")
                    );
                Context.Engine.RenderChildTemplate(
                    new CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>(new[] { Model.Data }, Model.Settings),
                    generationEnvironment,
                    Context,
                    new CreateChildTemplateByNameRequest("DefaultUsings")
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
                    model => Provider.Create(new CreateChildTemplateByModelRequest(model!.GetType().GetProperty(nameof(CsharpClassGeneratorViewModel<TypeBase>.Data))!.GetValue(model)))
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

    internal sealed class TypeBase
    {
        public string Namespace { get; set; } = "";
        public string Name { get; set; } = "";
        public TypeBase[]? SubClasses { get; set; }
        public string[] Usings { get; set; } = Array.Empty<string>();
    }
}
