namespace TemplateFramework.Abstractions;

public interface IChildTemplateContext
{
    ITemplateIdentifier Identifier { get; }
    object? Model { get; }
    int? IterationNumber { get; }
    int? IterationCount { get; }
}
