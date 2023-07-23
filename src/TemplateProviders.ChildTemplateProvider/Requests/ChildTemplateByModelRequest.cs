namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Requests;

public class ChildTemplateByModelRequest : ICreateTemplateRequest
{
    public ChildTemplateByModelRequest(object? model)
    {
        Model = model;
    }

    public object? Model { get; }
}
