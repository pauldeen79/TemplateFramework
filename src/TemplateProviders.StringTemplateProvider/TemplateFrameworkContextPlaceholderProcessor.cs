namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class TemplateFrameworkContextPlaceholderProcessor : INonGenericMember
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);

        if ((await context.Context.State["context"].ConfigureAwait(false)).Value is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return Result.Continue<GenericFormattableString>();
        }

        foreach (var placholderProcessor in templateFrameworkFormattableStringContext.Context.Expressions)
        {
            var result = await placholderProcessor.EvaluateAsync(context, token).ConfigureAwait(false);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        var functionName = context.FunctionCall.Name;

        if (templateFrameworkFormattableStringContext.ParametersDictionary.TryGetValue(functionName, out var parameterValue))
        {
            return Result.Success<GenericFormattableString>(parameterValue?.ToString() ?? string.Empty);
        }

        // Also return the parameter name, so GetParameters works.
        // For dynamically registered placeholders, make sure the name starts with two underscores, so it gets excluded here.
        if (!functionName.StartsWith("__", StringComparison.CurrentCulture))
        {
            templateFrameworkFormattableStringContext.ParameterNamesList.Add(functionName);
        }

        if (templateFrameworkFormattableStringContext.GetParametersOnly)
        {
            // When getting parameters, always return success, so the process always continues to get next parameters.
            return Result.Success<GenericFormattableString>(string.Empty);
        }

        return Result.Continue<GenericFormattableString>();
    }
}
