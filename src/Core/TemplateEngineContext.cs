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
    public ITemplateProvider Provider { get; }

    public TemplateEngineContext(IRenderTemplateRequest request, ITemplateEngine engine, ITemplateProvider provider, object template)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);
        Guard.IsNotNull(provider);
        Guard.IsNotNull(template);

        Identifier = request.Identifier;
        GenerationEnvironment = request.GenerationEnvironment;
        DefaultFilename = request.DefaultFilename;
        Model = request.Model;
        AdditionalParameters = request.AdditionalParameters;
        Context = request.Context;
        Engine = engine;
        Provider = provider;
        Template = template;
    }
}
