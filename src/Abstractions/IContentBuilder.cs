namespace TemplateFramework.Abstractions;

public interface IContentBuilder<out T> : IBuilder<IContent>
{
    string? Filename { get; set; }
    bool SkipWhenFileExists { get; set; }
    T Builder { get; }
}
