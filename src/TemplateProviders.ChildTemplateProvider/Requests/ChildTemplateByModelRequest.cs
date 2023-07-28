namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Requests;

public sealed class ChildTemplateByModelRequest : ICreateTemplateRequest
{
    public ChildTemplateByModelRequest(object? model)
    {
        Model = model;
    }

    public object? Model { get; }
}
