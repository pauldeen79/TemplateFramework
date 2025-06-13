namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class TemplateFrameworkContextFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);

        if ((await context.Context.State["context"].ConfigureAwait(false)).Value is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return Result.Continue<object?>();
        }

        foreach (var function in templateFrameworkFormattableStringContext.Context.Functions.OfType<INonGenericMember>())
        {
            var result = await function.EvaluateAsync(context, token).ConfigureAwait(false);
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
