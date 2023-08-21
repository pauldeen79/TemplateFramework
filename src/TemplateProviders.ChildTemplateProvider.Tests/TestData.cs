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

    internal abstract class CsharpClassGeneratorBase : IParameterizedTemplate, ITemplateContextContainer
    {
        // Properties that are injected by the template engine
        public ITemplateContext Context { get; set; } = default!;

        // Parameters, filled by the template engine
        public bool GenerateMultipleFiles { get; set; }
        public bool SkipWhenFileExists { get; set; }
        public bool CreateCodeGenerationHeader { get; set; } = true;
        public string? EnvironmentVersion { get; set; }
        public string? FilenamePrefix { get; set; }
        public string? FilenameSuffix { get; set; }
        public bool EnableNullableContext { get; set; }
        public int IndentCount { get; set; } = 1;
        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

        public ITemplateParameter[] GetParameters() => new[]
        {
            new TemplateParameter(nameof(GenerateMultipleFiles), typeof(bool)),
            new TemplateParameter(nameof(SkipWhenFileExists), typeof(bool)),
            new TemplateParameter(nameof(CreateCodeGenerationHeader), typeof(bool)),
            new TemplateParameter(nameof(EnvironmentVersion), typeof(string)),
            new TemplateParameter(nameof(FilenamePrefix), typeof(string)),
            new TemplateParameter(nameof(FilenameSuffix), typeof(string)),
            new TemplateParameter(nameof(EnableNullableContext), typeof(bool)),
            new TemplateParameter(nameof(IndentCount), typeof(int)),
            new TemplateParameter(nameof(CultureInfo), typeof(CultureInfo)),
        };

        public void SetParameter(string name, object? value)
        {
            switch (name)
            {
                case nameof(GenerateMultipleFiles):
                    GenerateMultipleFiles = Convert.ToBoolean(value, CultureInfo);
                    break;
                case nameof(SkipWhenFileExists):
                    SkipWhenFileExists = Convert.ToBoolean(value, CultureInfo);
                    break;
                case nameof(CreateCodeGenerationHeader):
                    CreateCodeGenerationHeader = Convert.ToBoolean(value, CultureInfo);
                    break;
                case nameof(EnvironmentVersion):
                    EnvironmentVersion = value?.ToString();
                    break;
                case nameof(FilenamePrefix):
                    FilenamePrefix = value?.ToString();
                    break;
                case nameof(FilenameSuffix):
                    FilenameSuffix = value?.ToString();
                    break;
                case nameof(EnableNullableContext):
                    EnableNullableContext = Convert.ToBoolean(value, CultureInfo);
                    break;
                case nameof(IndentCount):
                    IndentCount = Convert.ToInt32(value, CultureInfo);
                    break;
                case nameof(CultureInfo):
                    CultureInfo = value as CultureInfo ?? throw new NotSupportedException("CultureInfo parameter must be of type CultureInfo");
                    break;
                default:
                    throw new NotSupportedException($"Unsupported parameter: {name}");
            }
        }

        protected object AdditionalParameters
            => new
            {
                GenerateMultipleFiles,
                SkipWhenFileExists,
                CreateCodeGenerationHeader,
                EnvironmentVersion,
                FilenamePrefix,
                FilenameSuffix,
                EnableNullableContext,
                IndentCount,
                CultureInfo
            };
    }

    internal sealed class CsharpClassGenerator : CsharpClassGeneratorBase, IMultipleContentBuilderTemplate, IModelContainer<IEnumerable<TypeBase>>
    {
        // Properties that are injected by the template engine
        public IEnumerable<TypeBase>? Model { get; set; }

        public void Render(IMultipleContentBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            StringBuilder? singleStringBuilder = null;
            IGenerationEnvironment generationEnvironment = new MultipleContentBuilderEnvironment(builder);

            if (!GenerateMultipleFiles)
            {
                singleStringBuilder = builder.AddContent(Context.DefaultFilename, SkipWhenFileExists).Builder;
                generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
                Context.Engine.RenderChildTemplate(generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("CodeGenerationHeader")), Context.DefaultFilename, AdditionalParameters, Context);

                if (Context.IsRootContext)
                {
                    Context.Engine.RenderChildTemplate(Model, generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("DefaultUsings")), Context.DefaultFilename, AdditionalParameters, Context);
                }
            }

            foreach (var ns in Model.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                if (Context.IsRootContext && !GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine(CultureInfo, $"namespace {ns.Key}");
                    singleStringBuilder.AppendLine("{"); // open namespace
                }

                Context.Engine.RenderChildTemplates(ns.OrderBy(x => x.Name), generationEnvironment, typeBase => Context.Provider.Create(new ChildTemplateByModelRequest(typeBase)), Context.DefaultFilename, AdditionalParameters, Context);

                if (Context.IsRootContext && !GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine("}"); // close namespace
                }
            }
        }
    }

    internal sealed class CodeGenerationHeaderTemplate : CsharpClassGeneratorBase, IStringBuilderTemplate
    {
        public string Version
            => !string.IsNullOrEmpty(EnvironmentVersion)
                ? EnvironmentVersion
                : Environment.Version.ToString();

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);

            if (!CreateCodeGenerationHeader)
            {
                return;
            }

            builder.AppendLine(CultureInfo, $$"""
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

    internal sealed class DefaultUsingsTemplate : CsharpClassGeneratorBase, IModelContainer<IEnumerable<TypeBase>?>, IStringBuilderTemplate
    {
        public IEnumerable<TypeBase>? Model { get; set; }

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            var anyUsings = false;
            foreach (var @using in Usings)
            {
                builder.AppendLine(CultureInfo, $"using {@using};");
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

    internal sealed class ClassTemplate : CsharpClassGeneratorBase, IModelContainer<TypeBase>, IMultipleContentBuilderTemplate
    {
        public TypeBase? Model { get; set; }

        public void Render(IMultipleContentBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);
            Guard.IsNotNull(Context);

            StringBuilderEnvironment generationEnvironment;

            if (!GenerateMultipleFiles)
            {
                if (!builder.Contents.Any())
                {
                    builder.AddContent(Context.DefaultFilename, SkipWhenFileExists);
                }

                generationEnvironment = new StringBuilderEnvironment(builder.Contents.Last().Builder); // important to take the last contents, in case of sub classes
            }
            else
            {
                var filename = $"{FilenamePrefix}{Model.Name}{FilenameSuffix}.cs";
                var contentBuilder = builder.AddContent(filename, SkipWhenFileExists);
                generationEnvironment = new StringBuilderEnvironment(contentBuilder.Builder);
                Context.Engine.RenderChildTemplate(generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("CodeGenerationHeader")), Context.DefaultFilename, AdditionalParameters, Context);
                Context.Engine.RenderChildTemplate(Context.GetModelFromContextByType<IEnumerable<TypeBase>>() ?? throw new InvalidOperationException("No root context found"), generationEnvironment, Context.Provider.Create(new ChildTemplateByNameRequest("DefaultUsings")), Context.DefaultFilename, AdditionalParameters, Context);
                contentBuilder.Builder.AppendLine(CultureInfo, $"namespace {Model.Namespace}");
                contentBuilder.Builder.AppendLine("{");
            }

            if (EnableNullableContext && IndentCount == 1)
            {
                generationEnvironment.Builder.AppendLine("#nullable enable");
            }

            var indentedBuilder = new IndentedStringBuilder(generationEnvironment.Builder);
            for (int i = 0; i < IndentCount; i++)
            {
                indentedBuilder.Indent();
            }
            //TODO: Render attributes
            //TODO: Replace public class with more options based on model
            indentedBuilder.AppendLine($"public class {Model.Name}");
            indentedBuilder.AppendLine("{"); // open class

            //TODO: Render child items
            if (Model.SubClasses?.Any() == true)
            {
                var childAdditionalParameters = new
                {
                    GenerateMultipleFiles = false, //set to false because the sub classes will be generated as part of the current file
                    SkipWhenFileExists,
                    CreateCodeGenerationHeader,
                    EnvironmentVersion,
                    FilenamePrefix,
                    FilenameSuffix,
                    EnableNullableContext,
                    IndentCount = IndentCount + 1
                };

                var childTemplateInstance = new ClassTemplate();
                Context.Engine.RenderChildTemplates(Model.SubClasses, new MultipleContentBuilderEnvironment(builder), _ => childTemplateInstance, Context.DefaultFilename, childAdditionalParameters, Context);
            }

            indentedBuilder.AppendLine("}"); // close class

            if (EnableNullableContext && IndentCount == 1)
            {
                generationEnvironment.Builder.AppendLine("#nullable restore");
            }

            if (GenerateMultipleFiles)
            {
                generationEnvironment.Builder.AppendLine("}"); // close namespace
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
