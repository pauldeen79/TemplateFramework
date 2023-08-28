namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Requests;

public sealed class CreateFormattableStringTemplateRequest : ICreateTemplateRequest
{
    public CreateFormattableStringTemplateRequest(string template, IFormatProvider formatProvider)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(formatProvider);

        Template = template;
        FormatProvider = formatProvider;
    }

    public string Template { get; }
    public IFormatProvider FormatProvider { get; }
}
