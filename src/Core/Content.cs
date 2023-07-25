namespace TemplateFramework.Core;

internal sealed class Content : IContent
{
    public Content(string contents, bool skipWhenFileExists, string filename)
    {
        Guard.IsNotNull(contents);
        Guard.IsNotNullOrWhiteSpace(filename);

        Contents = contents;
        SkipWhenFileExists = skipWhenFileExists;
        Filename = filename;
    }

    public string Filename { get; }

    public bool SkipWhenFileExists { get; }

    public string Contents { get; }
}
