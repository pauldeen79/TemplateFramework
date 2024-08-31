namespace TemplateFramework.Core;

public class ContentBuilderOfStringBuilder : ContentBuilder<StringBuilder>
{
    public ContentBuilderOfStringBuilder()
    {
    }

    public ContentBuilderOfStringBuilder(StringBuilder builder) : base(builder)
    {
    }

    public override IContent Build() => new Content(Builder.ToString() ?? string.Empty, SkipWhenFileExists, Filename!);
}

public abstract class ContentBuilder<T> : IContentBuilder<T> where T : class, new()
{
    protected ContentBuilder() : this(new T())
    {
    }

    protected ContentBuilder(T builder)
    {
        Guard.IsNotNull(builder);
        Builder = builder;
    }

    public string? Filename { get; set; }
    public bool SkipWhenFileExists { get; set; }

    public T Builder { get; }

    public abstract IContent Build();
}
