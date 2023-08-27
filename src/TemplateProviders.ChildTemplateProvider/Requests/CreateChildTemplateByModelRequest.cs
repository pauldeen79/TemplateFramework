namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Requests;

public sealed class CreateChildTemplateByModelRequest : ICreateTemplateRequest
{
    public CreateChildTemplateByModelRequest(object? model)
    {
        Model = model;
    }

    public object? Model { get; }
}
