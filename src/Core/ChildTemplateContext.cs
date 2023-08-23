namespace TemplateFramework.Core;

public class ChildTemplateContext : IChildTemplateContext
{
    public ChildTemplateContext(object template) : this(template, null, null, null)
    {
    }

    public ChildTemplateContext(object template, object? model) : this(template, model, null, null)
    {
    }

    public ChildTemplateContext(object template, object? model, int? iterationNumber, int? iterationCount)
    {
        Guard.IsNotNull(template);

        Template = template;
        Model = model;
        IterationNumber = iterationNumber;
        IterationCount = iterationCount;
    }

    public object Template { get; }
    public object? Model { get; }

    public int? IterationNumber { get; set; }
    public int? IterationCount { get; set; }
}
