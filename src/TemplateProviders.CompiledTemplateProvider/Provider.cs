namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public class Provider : ITemplateProvider
{
    public bool Supports(ICreateTemplateRequest request) => request is CreateCompiledTemplateRequest;

    public object Create(ICreateTemplateRequest request)
    {
        Guard.IsNotNull(request);
        Guard.IsOfType<CreateCompiledTemplateRequest>(request);

        throw new NotImplementedException();
    }
}
