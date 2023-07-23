namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public class Provider : ITemplateProvider
{
    public bool Supports(ICreateTemplateRequest request) => request is CreateCompiledTemplateRequest;

    public object Create(ICreateTemplateRequest request)
    {
        Guard.IsNotNull(request);
        Guard.IsOfType<CreateCompiledTemplateRequest>(request);

        var createCompiledTemplateRequest = (CreateCompiledTemplateRequest)request;
        var assembly = AssemblyHelper.GetAssembly(createCompiledTemplateRequest.AssemblyName, createCompiledTemplateRequest.CurrentDirectory);
        var template = assembly.CreateInstance(createCompiledTemplateRequest.ClassName);
        if (template is null)
        {
            throw new InvalidOperationException($"Could not create instance of type {createCompiledTemplateRequest.ClassName}");
        }

        return template;
    }
}
