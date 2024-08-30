namespace TemplateFramework.Core;

public sealed class ContentBuilder<T> : IContentBuilder<T> where T : class, new()
{
    public ContentBuilder() : this(new T())
    {
    }

    public ContentBuilder(T builder)
    {
        Guard.IsNotNull(builder);
        Builder = builder;
    }

    public string? Filename { get; set; }
    public bool SkipWhenFileExists { get; set; }

    public T Builder { get; }

    public IContent Build() => new Content(Builder.ToString() ?? string.Empty, SkipWhenFileExists, Filename!);
}
