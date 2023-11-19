namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class TemplateFrameworkContextPlaceholderProcessor : IPlaceholderProcessor
{
    public int Order => 100;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        Guard.IsNotNull(value);
        Guard.IsNotNull(formatProvider);
        Guard.IsNotNull(formattableStringParser);

        if (context is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return Result.Continue<string>();
        }

        foreach (var placholderProcessor in templateFrameworkFormattableStringContext.Context.PlaceholderProcessors.OrderBy(x => Order))
        {
            var result = placholderProcessor.Process(value, formatProvider, context, formattableStringParser);
            if (result.IsSuccessful() && result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        if (templateFrameworkFormattableStringContext.ParametersDictionary.TryGetValue(value, out var parameterValue))
        {
            return Result.Success(parameterValue?.ToString() ?? string.Empty);
        }

        // Also return the parameter name, so GetParameters works.
        // For dynamically registered placeholders, make sure the name starts with two underscores, so it gets excluded here.
        if (!value.StartsWith("__", StringComparison.CurrentCulture))
        {
            templateFrameworkFormattableStringContext.ParameterNamesList.Add(value);
        }

        if (templateFrameworkFormattableStringContext.GetParametersOnly)
        {
            // When getting parameters, always return success, so the process always continues to get next parameters.
            return Result.Success(string.Empty);
        }

        return Result.Continue<string>();
    }
}
