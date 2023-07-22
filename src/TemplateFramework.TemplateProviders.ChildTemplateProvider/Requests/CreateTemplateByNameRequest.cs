namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Requests;

public class CreateTemplateByNameRequest : ICreateTemplateRequest
{
    public CreateTemplateByNameRequest(string name)
    {
        Guard.IsNotNullOrEmpty(name);

        Name = name;
    }

    public string Name { get; }
}
