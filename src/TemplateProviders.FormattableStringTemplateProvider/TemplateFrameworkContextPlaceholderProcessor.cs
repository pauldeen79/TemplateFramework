namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public sealed class TemplateFrameworkContextPlaceholderProcessor : IPlaceholderProcessor
{
    public int Order => 100;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context)
    {
        Guard.IsNotNull(value);
        Guard.IsNotNull(formatProvider);

        if (context is not TemplateFrameworkFormattableStringContext templateFrameworkFormattableStringContext)
        {
            return Result<string>.Continue();
        }

        foreach (var placholderProcessor in templateFrameworkFormattableStringContext.Processors.OrderBy(x => Order))
        {
            var result = placholderProcessor.Process(value, formatProvider, context);
            if (result.IsSuccessful() && result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        if (templateFrameworkFormattableStringContext.ParametersDictionary.TryGetValue(value, out var parameterValue))
        {
            return Result<string>.Success(parameterValue?.ToString() ?? string.Empty);
        }

        // Also return the parameter name, so GetParameters works.
        // For dynamically registered placeholders, make sure the name starts with two underscores, so it gets excluded here.
        if (!value.StartsWith("__", StringComparison.CurrentCulture))
        {
            templateFrameworkFormattableStringContext.ParameterNamesList.Add(value);
        }

        // Unknown parameter, let's just keep it empty for now
        return Result<string>.Success(string.Empty);
    }
}
