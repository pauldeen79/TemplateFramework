namespace TemplateFramework.Core;

public class TemplateEngineContext : ITemplateEngineContext
{
    public ITemplateIdentifier Identifier { get; }
    public IGenerationEnvironment GenerationEnvironment { get; }
    public string DefaultFilename { get; }
    public object? Model { get; }
    public object? AdditionalParameters { get; }
    public ITemplateContext? Context { get; }
    public object? Template { get; }
    public ITemplateEngine Engine { get; }

    public TemplateEngineContext(IRenderTemplateRequest request, ITemplateEngine engine, object template)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);
        Guard.IsNotNull(template);

        Identifier = request.Identifier;
        GenerationEnvironment = request.GenerationEnvironment;
        DefaultFilename = request.DefaultFilename;
        Model = request.Model;
        AdditionalParameters = request.AdditionalParameters;
        Context = request.Context;
        Engine = engine;
        Template = template;
    }
}
