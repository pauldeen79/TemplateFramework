namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

internal static class TestData
{
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

    internal sealed class StringBuilderTemplate : IStringBuilderTemplate
    {
        private readonly Action<StringBuilder> _delegate;

        public StringBuilderTemplate(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    internal sealed class MultipleContentBuilderTemplate : IMultipleContentBuilderTemplate
    {
        private readonly Action<IMultipleContentBuilder> _delegate;

        public MultipleContentBuilderTemplate(Action<IMultipleContentBuilder> @delegate) => _delegate = @delegate;

        public void Render(IMultipleContentBuilder builder) => _delegate(builder);
    }

    internal sealed class PlainTemplateWithAdditionalParameters : IParameterizedTemplate
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

    internal sealed class AdditionalParametersWrongTypeTemplate
    {
        public int GetParameters() => 0; // wrong result type!
    }

    internal sealed class MultipleContentBuilderTwoWrongArgumentsTemplate
    {
        private readonly Func<string> _delegate;

        public MultipleContentBuilderTwoWrongArgumentsTemplate(Func<string> @delegate)
        {
            _delegate = @delegate;
        }

        public void Render(IMultipleContentBuilder builder, int someWrongArgument) => throw new InvalidOperationException("This method is not supposed to be called, because the arguments don't match");

        public override string ToString() => _delegate();
    }

    internal sealed class MultipleContentBuilderOneWrongArgumentTemplate
    {
        private readonly Func<string> _delegate;

        public MultipleContentBuilderOneWrongArgumentTemplate(Func<string> @delegate)
        {
            _delegate = @delegate;
        }

        public void Render(int someWrongArgument) => throw new InvalidOperationException("This method is not supposed to be called, because the arguments don't match");

        public override string ToString() => _delegate();
    }
}
