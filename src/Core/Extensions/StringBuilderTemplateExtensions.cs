namespace TemplateFramework.Core.Extensions;

public static class StringBuilderTemplateExtensions
{
    public static void RenderToMultipleContentBuilder(this IStringBuilderTemplate instance,
                                                      IMultipleContentBuilder builder,
                                                      string defaultFilename)
        => instance.RenderToMultipleContentBuilder(builder, defaultFilename, false);

    public static void RenderToMultipleContentBuilder(this IStringBuilderTemplate instance,
                                                      IMultipleContentBuilder builder,
                                                      string defaultFilename,
                                                      bool skipWhenFileExists)
    {
        Guard.IsNotNull(builder);

        var stringBuilder = new StringBuilder();
        instance.Render(stringBuilder);

        var content = builder.Contents.FirstOrDefault(x => x.Filename == defaultFilename);
        if (content is not null)
        {
            content.Builder.Append(stringBuilder);
        }
        else
        {
            builder.AddContent(defaultFilename, skipWhenFileExists, stringBuilder);
        }
    }
}
