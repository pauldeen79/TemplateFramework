namespace TemplateFramework.Core.Tests;

public sealed class PlainTemplateWithAdditionalParameters : IParameterizedTemplate
{
    public string AdditionalParameter { get; set; } = "";

    public void SetParameter(string name, object? value)
    {
        if (name == nameof(AdditionalParameter))
        {
            AdditionalParameter = value?.ToString() ?? string.Empty;
        }
    }

    public ITemplateParameter[] GetParameters() => new[] { new TemplateParameter(nameof(AdditionalParameter), typeof(string)) };

    public override string ToString() => AdditionalParameter;
}

public sealed class TestTemplateComponentRegistryPlugin : ITemplateComponentRegistryPlugin
{
    public void Initialize(ITemplateComponentRegistry registry)
    {
    }
}

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

    internal sealed class Template : IStringBuilderTemplate
    {
        private readonly Action<StringBuilder> _delegate;

        public Template(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    internal sealed class TemplateWithModel<T> : IStringBuilderTemplate, IModelContainer<T>
    {
        public T? Model { get; set; } = default!;

        private readonly Action<StringBuilder> _delegate;

        public TemplateWithModel(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    internal sealed class TemplateWithDefaultFilename : IStringBuilderTemplate, IDefaultFilenameContainer
    {
        private readonly Action<StringBuilder> _delegate;

        public TemplateWithDefaultFilename(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public string DefaultFilename { get; set; } = "";

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    internal sealed class TemplateWithViewModel<T> : IStringBuilderTemplate, IParameterizedTemplate
    {
        public T? ViewModel { get; set; } = default!;

        private readonly Action<StringBuilder> _delegate;

        public TemplateWithViewModel(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);

        public void SetParameter(string name, object? value) // this is added in case of viewmodels which don't have a public parameterless constructor
        {
            if (name == nameof(ViewModel))
            {
                ViewModel = (T?)value;
            }
        }

        public ITemplateParameter[] GetParameters() => new[] { new TemplateParameter(nameof(ViewModel), typeof(T?)) };
    }

    internal sealed class PlainTemplateWithModelAndAdditionalParameters<T> : IModelContainer<T>, IParameterizedTemplate
    {
        public T? Model { get; set; } = default!;

        public string AdditionalParameter { get; set; } = "";

        public void SetParameter(string name, object? value)
        {
            if (name == nameof(AdditionalParameter))
            {
                AdditionalParameter = value?.ToString() ?? string.Empty;
            }
        }

        public ITemplateParameter[] GetParameters() => new[] { new TemplateParameter(nameof(AdditionalParameter), typeof(T?)) };

        public override string ToString() => AdditionalParameter;
    }

    internal sealed class PlainTemplateWithTemplateContext : ITemplateContextContainer
    {
        private readonly Func<ITemplateContext, string> _delegate;

        public PlainTemplateWithTemplateContext(Func<ITemplateContext, string> @delegate) => _delegate = @delegate;

        public ITemplateContext Context { get; set; } = default!;

        public override string ToString() => _delegate(Context);
    }

    internal sealed class TextTransformTemplate : ITextTransformTemplate
    {
        private readonly Func<string> _delegate;

        public TextTransformTemplate(Func<string> @delegate) => _delegate = @delegate;

        public string TransformText() => _delegate();
    }

    /// <summary>
    /// Example of a ViewModel class without a public parameterless constructor. This one can't be initialized by the TemplateInitializer.
    /// </summary>
    internal sealed class NonConstructableViewModel : IParameterizedTemplate
    {
        public NonConstructableViewModel(string property)
        {
            Property = property;
        }

        public string Property { get; set; }

        public void SetParameter(string name, object? value)
        {
            if (name == nameof(Property))
            {
                Property = value?.ToString() ?? string.Empty;
            }
        }

        public ITemplateParameter[] GetParameters() => new[] { new TemplateParameter(nameof(Property), typeof(string)) };
    }

    internal sealed class PocoParameterizedTemplate
    {
        public string Parameter { get; set; } = "";

        public override string ToString() => Parameter;
    }
}
