namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Requests;

public class ChildTemplateByNameRequest : ICreateTemplateRequest
{
    public ChildTemplateByNameRequest(string name)
    {
        Guard.IsNotNullOrEmpty(name);

        Name = name;
    }

    public string Name { get; }
}
