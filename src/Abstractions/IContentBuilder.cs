namespace TemplateFramework.Abstractions;

public interface IContentBuilder<out T>
{
    string? Filename { get; set; }
    bool SkipWhenFileExists { get; set; }
    T Builder { get; }
    IContent Build();
}
