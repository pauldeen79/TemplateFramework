namespace TemplateFramework.Core.Extensions;

public static class TemplateEngineExtensions
{
    public static void Render(this ITemplateEngine instance, IRenderTemplateRequest request)
    {
        Guard.IsNotNull(instance);
        Guard.IsNotNull(request);

        var type = request.GetType();

        if (type.IsGenericType)
        {
            instance.GetType().GetMethod(nameof(ITemplateEngine.Render))!.MakeGenericMethod(type.GetGenericArguments()[0]).Invoke(instance, new[] { request });
        }
        else
        {
            instance.Render(new RenderTemplateRequest<object?>(request));
        }
    }
}
