namespace TemplateFramework.Abstractions.Templates;

public interface ITextTransformTemplate
{
    Task<string> TransformTextAsync(CancellationToken token);
}
