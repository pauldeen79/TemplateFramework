namespace TemplateFramework.Core.TemplateRenderers;

public sealed class StringBuilderTemplateRenderer : BuilderTemplateRendererBase<StringBuilderEnvironment, StringBuilder>
{
    public StringBuilderTemplateRenderer(IEnumerable<IBuilderTemplateRenderer<StringBuilder>> renderers) : base(renderers)
    {
    }

    protected override Task<Result> DefaultImplementation(object templateInstance, StringBuilder builder)
    {
        var output = templateInstance.ToString();

        if (!string.IsNullOrEmpty(output))
        {
            builder.Append(output);
        }

        return Task.FromResult(Result.Success());
    }
}
