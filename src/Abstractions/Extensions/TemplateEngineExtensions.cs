namespace TemplateFramework.Abstractions.Extensions;

public static class TemplateEngineExtensions
{
    public static Task<Result> RenderAsync(this ITemplateEngine instance, IRenderTemplateRequest request)
        => instance.RenderAsync(request, CancellationToken.None);

    public static Task<Result<ITemplateParameter[]>> GetParametersAsync(this ITemplateEngine instance, object templateInstance)
        => instance.GetParametersAsync(templateInstance, CancellationToken.None);
}
