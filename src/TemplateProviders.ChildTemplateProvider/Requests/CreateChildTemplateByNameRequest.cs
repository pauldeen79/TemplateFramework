namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Requests;

public sealed class CreateChildTemplateByNameRequest : ICreateTemplateRequest
{
    public CreateChildTemplateByNameRequest(string name)
    {
        Guard.IsNotNullOrEmpty(name);

        Name = name;
    }

    public string Name { get; }
}
