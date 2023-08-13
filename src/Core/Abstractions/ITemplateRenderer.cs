namespace TemplateFramework.Core.Abstractions;

public interface ITemplateRenderer
{
    bool Supports(IGenerationEnvironment generationEnvironment);
    void Render(IRenderTemplateRequest request);
}
