namespace TemplateFramework.Core.BuilderTemplateRenderers;

public class TypedBuilderTemplateRenderer<TBuilder> : IBuilderTemplateRenderer<TBuilder>
{
    public async Task<Result> TryRenderAsync(object instance, TBuilder builder, CancellationToken token)
    {
        Guard.IsNotNull(builder);

        if (instance is IBuilderTemplate<TBuilder> typedTemplate)
        {
            return await typedTemplate.RenderAsync(builder, token).ConfigureAwait(false);
        }

        return Result.Continue();
    }
}
