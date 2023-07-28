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
            var modelProperty = type.GetProperty(nameof(IRenderTemplateRequest<object?>.Model));
            if (modelProperty != null)
            {
                var modelValue = modelProperty.GetValue(request);
                if (modelValue != null)
                {
                    var typedRequest = Activator.CreateInstance(typeof(RenderTemplateRequest<>).MakeGenericType(modelValue.GetType()), request.Template, request.GenerationEnvironment, modelValue, request.DefaultFilename, request.AdditionalParameters, request.Context);
                    instance.GetType().GetMethod(nameof(ITemplateEngine.Render))!.MakeGenericMethod(modelValue.GetType()).Invoke(instance, new[] { typedRequest });
                    return;
                }
            }
        }

        instance.Render(new RenderTemplateRequest<object?>(request));
    }
}
