namespace TemplateFramework.Core.Extensions;

public static class StringBuilderExtensions
{
    public static void AppendMultipleContents(this StringBuilder stringBuilder, IMultipleContent output, string basePath)
    {
        Guard.IsNotNull(output);

        foreach (var content in output.Contents)
        {
            var path = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(content.Filename)
                ? content.Filename
                : Path.Combine(basePath, content.Filename);

            stringBuilder.Append(path);
            stringBuilder.AppendLine(":");
            stringBuilder.AppendLine(content.Contents);
        }
    }
}
