namespace TemplateFramework.Abstractions;

public interface IChildTemplateContext
{
    object Template { get; }
    object? Model { get; }
    int? IterationNumber { get; }
    int? IterationCount { get; }
}
