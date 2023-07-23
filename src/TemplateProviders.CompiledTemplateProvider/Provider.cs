using TemplateFramework.Runtime;

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
            throw new NotSupportedException($"Class [{createCompiledTemplateRequest.ClassName}] from assembly [{createCompiledTemplateRequest.AssemblyName}] could not be instanciated");
        }

        return template;
    }
}
