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

    internal sealed record CsharpClassGeneratorSettings(bool GenerateMultipleFiles,
                                                 bool SkipWhenFileExists,
                                                 bool CreateCodeGenerationHeader,
                                                 string? EnvironmentVersion,
                                                 string? FilenamePrefix,
                                                 string? FilenameSuffix,
                                                 bool EnableNullableContext,
                                                 int IndentCount,
                                                 CultureInfo CultureInfo);

    internal abstract class CsharpClassGeneratorBase : ITemplateContextContainer
    {
        // Properties that are injected by the template engine
        public ITemplateContext Context { get; set; } = default!;
    }

    internal sealed class CsharpClassGenerator : CsharpClassGeneratorBase, IMultipleContentBuilderTemplate, IModelContainer<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>
    {
        // Properties that are injected by the template engine
        public CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>? Model { get; set; }

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
                Context.Engine.RenderChildTemplate(Model.Settings, generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("CodeGenerationHeader")), Context.DefaultFilename, Context);

                if (Context.IsRootContext)
                {
                    Context.Engine.RenderChildTemplate(Model, generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("DefaultUsings")), Context.DefaultFilename, Context);
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

                Context.Engine.RenderChildTemplates(typeBaseItems, generationEnvironment, typeBase => Context.Provider.Create(new ChildTemplateByModelRequest(((CsharpClassGeneratorViewModel<TypeBase>)typeBase!).Data)), Context.DefaultFilename, Context);

                if (Context.IsRootContext && !Model.Settings.GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine("}"); // close namespace
                }
            }
        }
    }

    internal sealed class CodeGenerationHeaderTemplate : CsharpClassGeneratorBase, IStringBuilderTemplate, IModelContainer<CsharpClassGeneratorSettings>
    {
        public CsharpClassGeneratorSettings? Model { get; set; }

        public string Version
            => !string.IsNullOrEmpty(Model?.EnvironmentVersion)
                ? Model.EnvironmentVersion
                : Environment.Version.ToString();

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);

            if (Model?.CreateCodeGenerationHeader != true)
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

    internal sealed class DefaultUsingsTemplate : CsharpClassGeneratorBase, IModelContainer<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IStringBuilderTemplate
    {
        public CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>? Model { get; set; }

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
                //.Union(Model.SelectMany(classItem => classItem.Metadata.GetStringValues(ModelFramework.Objects.MetadataNames.CustomUsing)))
                .OrderBy(ns => ns)
                .Distinct();
    }

    internal sealed class ClassTemplate : CsharpClassGeneratorBase, IModelContainer<CsharpClassGeneratorViewModel<TypeBase>>, IMultipleContentBuilderTemplate
    {
        public CsharpClassGeneratorViewModel<TypeBase>? Model { get; set; }

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
                Context.Engine.RenderChildTemplate(Model.Settings, generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("CodeGenerationHeader")), Context.DefaultFilename, Context);
                var rootItems = Context.GetModelFromContextByType<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>(context => context.IsRootContext)
                    ?? throw new InvalidOperationException("No root context found");
                Context.Engine.RenderChildTemplate(rootItems, generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("DefaultUsings")), Context.DefaultFilename, Context);
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
                var settings = new CsharpClassGeneratorSettings
                (
                    GenerateMultipleFiles: false, //set to false because the sub classes will be generated as part of the current file
                    SkipWhenFileExists: Model.Settings.SkipWhenFileExists,
                    CreateCodeGenerationHeader: Model.Settings.CreateCodeGenerationHeader,
                    EnvironmentVersion: Model.Settings.EnvironmentVersion,
                    FilenamePrefix: Model.Settings.FilenamePrefix,
                    FilenameSuffix: Model.Settings.FilenameSuffix,
                    EnableNullableContext: Model.Settings.EnableNullableContext,
                    IndentCount: Model.Settings.IndentCount + 1,
                    CultureInfo: Model.Settings.CultureInfo
                );

                var childTemplateInstance = new ClassTemplate();
                Context.Engine.RenderChildTemplates(Model.Data.SubClasses.Select(typeBase => new CsharpClassGeneratorViewModel<TypeBase>(typeBase, settings)), new MultipleContentBuilderEnvironment(builder), _ => childTemplateInstance, Context.DefaultFilename, Context);
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
    }
}
