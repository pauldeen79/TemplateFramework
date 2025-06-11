namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class TemplateFrameworkContextExpressionComponent : IExpressionComponent
{
    public int Order => 99;

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);

        if ((await context.State["context"].ConfigureAwait(false)).Value is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return Result.Continue<GenericFormattableString>();
        }

        foreach (var expressionComponent in templateFrameworkFormattableStringContext.Context.Expressions)
        {
            var result = await expressionComponent.EvaluateAsync(context, token).ConfigureAwait(false);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        if (templateFrameworkFormattableStringContext.ParametersDictionary.TryGetValue(context.Expression, out var parameterValue))
        {
            return Result.Success<GenericFormattableString>(parameterValue?.ToString() ?? string.Empty);
        }

        // Also return the parameter name, so GetParameters works.
        // For dynamically registered placeholders, make sure the name starts with two underscores, so it gets excluded here.
        if (!context.Expression.StartsWith("__", StringComparison.CurrentCulture))
        {
            templateFrameworkFormattableStringContext.ParameterNamesList.Add(context.Expression);
        }

        if (templateFrameworkFormattableStringContext.GetParametersOnly)
        {
            // When getting parameters, always return success, so the process always continues to get next parameters.
            return Result.Success<GenericFormattableString>(string.Empty);
        }

        return Result.Continue<GenericFormattableString>();
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(GetType())
            .WithSourceExpression(context.Expression);

        if ((await context.State["context"].ConfigureAwait(false)).Value is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        foreach (var expressionComponent in templateFrameworkFormattableStringContext.Context.Expressions)
        {
            var expressionResult = await expressionComponent.ParseAsync(context, token).ConfigureAwait(false)
                ?? new ExpressionParseResultBuilder()
                    .WithExpressionComponentType(GetType())
                    .WithSourceExpression(context.Expression)
                    .WithStatus(ResultStatus.Error)
                    .WithErrorMessage($"ExpressinComponent {expressionComponent.GetType().FullName} returned null on ParseAsync call");
            
            if (expressionResult.Status != ResultStatus.Continue)
            {
                return expressionResult;
            }
        }

        if (templateFrameworkFormattableStringContext.ParametersDictionary.TryGetValue(context.Expression, out var parameterValue))
        {
            return result
                .WithStatus(ResultStatus.Ok)
                .WithResultType(parameterValue?.GetType());
        }

        // Also return the parameter name, so GetParameters works.
        // For dynamically registered placeholders, make sure the name starts with two underscores, so it gets excluded here.
        if (!context.Expression.StartsWith("__", StringComparison.CurrentCulture))
        {
            templateFrameworkFormattableStringContext.ParameterNamesList.Add(context.Expression);
        }

        if (templateFrameworkFormattableStringContext.GetParametersOnly)
        {
            // When getting parameters, always return success, so the process always continues to get next parameters.
            return result
                .WithStatus(ResultStatus.Ok)
                .WithResultType(typeof(GenericFormattableString));
        }

        return result.WithStatus(ResultStatus.Continue);
    }
}
