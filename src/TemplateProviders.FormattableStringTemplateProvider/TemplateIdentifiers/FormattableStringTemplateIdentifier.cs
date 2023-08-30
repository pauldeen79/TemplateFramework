namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.TemplateIdentifiers;

public sealed class FormattableStringTemplateIdentifier : ITemplateIdentifier
{
    public FormattableStringTemplateIdentifier(string template, IFormatProvider formatProvider)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(formatProvider);

        Template = template;
        FormatProvider = formatProvider;
    }

    public string Template { get; }
    public IFormatProvider FormatProvider { get; }
}
