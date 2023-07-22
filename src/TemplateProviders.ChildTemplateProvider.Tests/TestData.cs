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

    internal sealed class MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine : IMultipleContentBuilderTemplate, ITemplateContextContainer, ITemplateEngineContainer
    {
        private readonly Action<IMultipleContentBuilder, ITemplateContext, ITemplateEngine, ITemplateProvider> _delegate;

        public MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(ITemplateProvider templateProvider,
                                                                                  Action<IMultipleContentBuilder, ITemplateContext, ITemplateEngine, ITemplateProvider> @delegate)
        {
            TemplateProvider = templateProvider;
            _delegate = @delegate;
        }

        public ITemplateContext Context { get; set; } = default!;
        public ITemplateEngine TemplateEngine { get; set; } = default!;
        public ITemplateProvider TemplateProvider { get; }

        public void Render(IMultipleContentBuilder builder) => _delegate(builder, Context, TemplateEngine, TemplateProvider);
    }
}
