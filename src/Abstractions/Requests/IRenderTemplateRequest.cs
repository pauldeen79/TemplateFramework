namespace TemplateFramework.Abstractions.Requests;

public interface IRenderTemplateRequest
{
    object Template { get; }
    IGenerationEnvironment GenerationEnvironment { get; }
    string DefaultFilename { get; }
    object? AdditionalParameters { get; }
    ITemplateContext? Context { get; }
}

public interface IRenderTemplateRequest<out TModel> : IRenderTemplateRequest
{
    TModel? Model { get; }
}
