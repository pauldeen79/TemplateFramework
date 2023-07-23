namespace TemplateFramework.Abstractions;

public interface IContent
{
    string Filename { get; }
    bool SkipWhenFileExists { get; }
    string Contents { get; }
}
