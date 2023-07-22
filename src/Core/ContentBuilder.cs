namespace TemplateFramework.Core;

internal sealed class ContentBuilder : IContentBuilder
{
    public ContentBuilder() : this(new StringBuilder())
    {
    }

    public ContentBuilder(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Builder = builder;
    }

    public string? Filename { get; set; }
    public bool SkipWhenFileExists { get; set; }

    public StringBuilder Builder { get; }

    public IContent Build() => new Content(Builder, SkipWhenFileExists, Filename);
}
