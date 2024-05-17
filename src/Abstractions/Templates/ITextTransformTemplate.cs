namespace TemplateFramework.Abstractions.Templates;

public interface ITextTransformTemplate
{
    Task<string> TransformText(CancellationToken cancellationToken);
}
