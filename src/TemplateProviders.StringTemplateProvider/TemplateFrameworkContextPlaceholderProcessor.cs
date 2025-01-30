namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class TemplateFrameworkContextPlaceholderProcessor : IPlaceholder
{
    public Result<GenericFormattableString> Evaluate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
    {
        Guard.IsNotNull(value);
        Guard.IsNotNull(settings);
        Guard.IsNotNull(formattableStringParser);

        if (context is not TemplateFrameworkStringContext templateFrameworkFormattableStringContext)
        {
            return Result.Continue<GenericFormattableString>();
        }

        foreach (var placholderProcessor in templateFrameworkFormattableStringContext.Context.Placeholders)
        {
            var result = placholderProcessor.Evaluate(value, settings, context, formattableStringParser);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        if (templateFrameworkFormattableStringContext.ParametersDictionary.TryGetValue(value, out var parameterValue))
        {
            return Result.Success<GenericFormattableString>(parameterValue?.ToString() ?? string.Empty);
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
            return Result.Success<GenericFormattableString>(string.Empty);
        }

        return Result.Continue<GenericFormattableString>();
    }

    public Result Validate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser)
    {
        return Result.Success();
    }
}
