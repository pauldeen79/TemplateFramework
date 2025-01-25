namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class TemplateFrameworkContextFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        Guard.IsNotNull(context);

        if (context.Context is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return Result.Continue<object?>();
        }

        foreach (var function in templateFrameworkFormattableStringContext.Context.Functions)
        {
            var result = function.Evaluate(context);
            if (!result.IsSuccessful())
            {
                return result;
            }

            if (result.Status == ResultStatus.Continue)
            {
                continue;
            }

            return result;
        }

        // No custom FunctionResultParsers registered, the parent should continue to try other FunctionResultParsers statically injected into the ServiceCollection on application startup.
        return Result.Continue<object?>();
    }
}
