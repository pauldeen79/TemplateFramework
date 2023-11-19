namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class TemplateFrameworkContextFunctionResultParser : IFunctionResultParser
{
    public Result<object?> Parse(FunctionParseResult functionParseResult, object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser)
    {
        Guard.IsNotNull(functionParseResult);
        Guard.IsNotNull(evaluator);

        if (context is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return Result.Continue<object?>();
        }

        foreach (var functionResultParser in templateFrameworkFormattableStringContext.Context.FunctionResultParsers)
        {
            var result = functionResultParser.Parse(functionParseResult, context, evaluator, parser);
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
