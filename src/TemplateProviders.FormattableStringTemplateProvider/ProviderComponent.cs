namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider;

public class ProviderComponent : ITemplateProviderComponent
{
    private readonly IFormattableStringParser _formattableStringParser;

    public ProviderComponent(IFormattableStringParser formattableStringParser)
    {
        Guard.IsNotNull(formattableStringParser);

        _formattableStringParser = formattableStringParser;
    }

    public bool Supports(ITemplateIdentifier request) => request is CreateFormattableStringTemplateRequest;

    public object Create(ITemplateIdentifier request)
    {
        Guard.IsNotNull(request);
        Guard.IsOfType<CreateFormattableStringTemplateRequest>(request);

        var createFormattableStringTemplateRequest = (CreateFormattableStringTemplateRequest)request;

        return new FormattableStringTemplate(createFormattableStringTemplateRequest, _formattableStringParser);
    }
}
