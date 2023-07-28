namespace TemplateFramework.Abstractions.Requests;

public interface IRenderTemplateRequest
{
    object Template { get; }
    IGenerationEnvironment GenerationEnvironment { get; }
    string DefaultFilename { get; }
    object? Model { get; }
    object? AdditionalParameters { get; }
    ITemplateContext? Context { get; }
}
