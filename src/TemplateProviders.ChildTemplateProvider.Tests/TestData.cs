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

    internal sealed class BogusCsharpClassGenerator : IMultipleContentBuilderTemplate, IParameterizedTemplate, ITemplateContextContainer, ITemplateEngineContainer, ITemplateProviderContainer, IDefaultFilenameContainer, IModelContainer<IEnumerable<TypeBase>>
    {
        public BogusCsharpClassGenerator()
        {
            // Provide default values for parameters, if needed
            GenerateMultipleFiles = false;
            SkipWhenFileExists = false;
            CreateCodeGenerationHeader = true;
        }

        // Properties that are injected by the template engine
        public ITemplateContext Context { get; set; } = default!;
        public ITemplateEngine Engine { get; set; } = default!;
        public ITemplateProvider Provider { get; set; } = default!;
        public string DefaultFilename { get; set; } = default!;
        public IEnumerable<TypeBase>? Model { get; set; }

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
                Engine.RenderChildTemplate(this, generationEnvironment, Provider.Create(new ChildTemplateByNameRequest("CodeGenerationHeader")), DefaultFilename, Context);

                if (Context.IsRootContext)
                {
                    Engine.RenderChildTemplate(this, generationEnvironment, Provider.Create(new ChildTemplateByNameRequest("DefaultUsings")), DefaultFilename, Context);
                }
            }

            foreach (var ns in Model.GroupBy(x => x.Namespace))
            {
                if (Context.IsRootContext && !GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine(CultureInfo.InvariantCulture, $"namespace {ns.Key}");
                    singleStringBuilder!.AppendLine("{"); // open namespace
                }

                Engine.RenderChildTemplates(ns, generationEnvironment, typeBase => Provider.Create(new ChildTemplateByModelRequest(typeBase)), DefaultFilename, Context);

                if (Context.IsRootContext && !GenerateMultipleFiles)
                {
                    singleStringBuilder!.AppendLine("}"); // close namespace
                }
            }
        }

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
    }

    internal sealed class CodeGenerationHeaderTemplate : IModelContainer<BogusCsharpClassGenerator>, IStringBuilderTemplate
    {
        public BogusCsharpClassGenerator? Model { get; set; }

        public void Render(StringBuilder builder)
        {
            Guard.IsNotNull(builder);
            Guard.IsNotNull(Model);

            if (!Model.CreateCodeGenerationHeader)
            {
                return;
            }

            var version = !string.IsNullOrEmpty(Model.EnvironmentVersion)
                ? Model.EnvironmentVersion
                : Environment.Version.ToString();

            builder.AppendLine(CultureInfo.InvariantCulture, $@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: { version }
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------");
        }
    }

    internal sealed class DefaultUsingsTemplate : IModelContainer<BogusCsharpClassGenerator>, IStringBuilderTemplate
    {
        public BogusCsharpClassGenerator? Model { get; set; }

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
