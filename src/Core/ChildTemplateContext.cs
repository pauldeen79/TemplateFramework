namespace TemplateFramework.Core;

public class ChildTemplateContext : IChildTemplateContext
{
    public ChildTemplateContext(ITemplateIdentifier identifier) : this(identifier, null, null, null)
    {
    }

    public ChildTemplateContext(ITemplateIdentifier identifier, object? model) : this(identifier, model, null, null)
    {
    }

    public ChildTemplateContext(ITemplateIdentifier identifier, object? model, int? iterationNumber, int? iterationCount)
    {
        Guard.IsNotNull(identifier);

        Identifier = identifier;
        Model = model;
        IterationNumber = iterationNumber;
        IterationCount = iterationCount;
    }

    public ITemplateIdentifier Identifier { get; }
    public object? Model { get; }

    public int? IterationNumber { get; set; }
    public int? IterationCount { get; set; }
}
