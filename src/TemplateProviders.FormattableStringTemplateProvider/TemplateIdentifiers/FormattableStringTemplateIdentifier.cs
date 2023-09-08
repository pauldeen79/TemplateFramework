namespace TemplateFramework.TemplateProviders.StringTemplateProvider.TemplateIdentifiers;

public sealed class FormattableStringTemplateIdentifier : ITemplateComponentRegistryIdentifier
{
    public FormattableStringTemplateIdentifier(
        string template,
        IFormatProvider formatProvider) : this(template, formatProvider, null, null, string.Empty)
    {
    }


    public FormattableStringTemplateIdentifier(
        string template,
        IFormatProvider formatProvider,
        string? pluginAssemblyName,
        string? pluginClassName,
        string? currentDirectory)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(formatProvider);

        if (string.IsNullOrEmpty(currentDirectory))
        {
            currentDirectory = Directory.GetCurrentDirectory();
        }

        Template = template;
        FormatProvider = formatProvider;
        PluginAssemblyName = pluginAssemblyName;
        PluginClassName = pluginClassName;
        CurrentDirectory = currentDirectory;
    }

    public string Template { get; }
    public IFormatProvider FormatProvider { get; }

    public string? PluginAssemblyName { get; }
    public string? PluginClassName { get; }
    public string CurrentDirectory { get; }
}
