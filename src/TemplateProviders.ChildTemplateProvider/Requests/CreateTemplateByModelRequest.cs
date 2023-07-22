namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Requests;

public class CreateTemplateByModelRequest : ICreateTemplateRequest
{
    public CreateTemplateByModelRequest(object? model)
    {
        Model = model;
    }

    public object? Model { get; }
}
