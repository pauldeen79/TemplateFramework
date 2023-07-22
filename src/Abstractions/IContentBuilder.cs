namespace TemplateFramework.Abstractions;

public interface IContentBuilder
{
    string? Filename { get; set; }
    bool SkipWhenFileExists { get; set; }
    StringBuilder Builder { get; }
    IContent Build();
}
