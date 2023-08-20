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

    internal sealed class MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine : IMultipleContentBuilderTemplate, ITemplateContextContainer, ITemplateEngineContainer, ITemplateProviderContainer
    {
        private readonly Action<IMultipleContentBuilder, ITemplateContext, ITemplateEngine, ITemplateProvider> _delegate;

        public MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(Action<IMultipleContentBuilder, ITemplateContext, ITemplateEngine, ITemplateProvider> @delegate)
        {
            _delegate = @delegate;
        }

        public ITemplateContext Context { get; set; } = default!;
        public ITemplateEngine Engine { get; set; } = default!;
        public ITemplateProvider Provider { get; set; } = default!;

        public void Render(IMultipleContentBuilder builder) => _delegate(builder, Context, Engine, Provider);
    }

    internal abstract class CsharpClassGeneratorBase : IParameterizedTemplate, ITemplateContextContainer, ITemplateEngineContainer, ITemplateProviderContainer, IDefaultFilenameContainer
    {
        protected CsharpClassGeneratorBase()
        {
            // Provide default values for parameters, if needed
            GenerateMultipleFiles = false;
            SkipWhenFileExists = false;
            CreateCodeGenerationHeader = true;
            EnvironmentVersion = null;
        }

        // Properties that are injected by the template engine
        public ITemplateContext Context { get; set; } = default!;
        public ITemplateEngine Engine { get; set; } = default!;
        public ITemplateProvider Provider { get; set; } = default!;
        public string DefaultFilename { get; set; } = default!;

        // Parameters, filled by the template engine
        public bool GenerateMultipleFiles { get; set; }
        public bool SkipWhenFileExists { get; set; }
        public bool CreateCodeGenerationHeader { get; set; }
        public string? EnvironmentVersion { get; set; }

        public ITemplateParameter[] GetParameters() => new[]
        {
            new TemplateParameter(nameof(GenerateMultipleFiles), typeof(bool)),
            new TemplateParameter(nameof(SkipWhenFileExists), typeof(bool)),
            new TemplateParameter(nameof(CreateCodeGenerationHeader), typeof(bool)),
            new TemplateParameter(nameof(EnvironmentVersion), typeof(string)),
        };

        public void SetParameter(string name, object? value)
        {
            switch (name)
            {
                case nameof(GenerateMultipleFiles):
                    GenerateMultipleFiles = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                    break;
                case nameof(SkipWhenFileExists):
                    SkipWhenFileExists = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                    break;
                case nameof(CreateCodeGenerationHeader):
                    CreateCodeGenerationHeader = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                    break;
                case nameof(EnvironmentVersion):
                    EnvironmentVersion = value?.ToString();
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
            Guard.IsNotNull(DefaultFilename);

            StringBuilder? singleStringBuilder = null;
            IGenerationEnvironment generationEnvironment = new MultipleContentBuilderEnvironment(builder);

            if (!GenerateMultipleFiles)
            {
                singleStringBuilder = builder.AddContent(DefaultFilename, SkipWhenFileExists).Builder;
                generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
            }

            if (!GenerateMultipleFiles)
            {
                Engine.RenderChildTemplate(generationEnvironment, Provider.Create(new ChildTemplateByNameRequest("CodeGenerationHeader")), DefaultFilename, AdditionalParameters, Context);

                if (Context.IsRootContext)
                {
                    Engine.RenderChildTemplate(generationEnvironment, Provider.Create(new ChildTemplateByNameRequest("DefaultUsings")), DefaultFilename, AdditionalParameters, Context);
                }
            }

            foreach (var ns in Model.GroupBy(x => x.Namespace))
            {
                if (Context.IsRootContext && !GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine(CultureInfo.InvariantCulture, $"namespace {ns.Key}");
                    singleStringBuilder.AppendLine("{"); // open namespace
                }

                Engine.RenderChildTemplates(ns, generationEnvironment, typeBase => Provider.Create(new ChildTemplateByModelRequest(typeBase)), DefaultFilename, AdditionalParameters, Context);

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

            builder.AppendLine(CultureInfo.InvariantCulture, $@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: { Version }
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------");
        }
    }

    internal sealed class DefaultUsingsTemplate : IModelContainer<IEnumerable<TypeBase>?>, IStringBuilderTemplate
    {
        public IEnumerable<TypeBase>? Model { get; set; }

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            //TODO: Implement
        }
    }

    internal sealed class ClassTemplate : IModelContainer<TypeBase>, IStringBuilderTemplate
    {
        public TypeBase? Model { get; set; }

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            //TODO: Implement
        }
    }

    internal sealed class TypeBase
    {
        public string Namespace { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
