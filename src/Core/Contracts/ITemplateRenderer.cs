namespace TemplateFramework.Core.Contracts;

public interface ITemplateRenderer
{
    bool Supports(IGenerationEnvironment generationEnvironment);
    void Render(IRenderTemplateRequest request);
}
