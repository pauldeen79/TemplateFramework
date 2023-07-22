namespace TemplateFramework.Abstractions.Extensions;

public static class TemplateFileManagerExtensions
{
    public static StringBuilder StartNewFile(this ITemplateFileManager instance)
        => instance.StartNewFile(string.Empty, false);

    public static StringBuilder StartNewFile(this ITemplateFileManager instance, string filename)
        => instance.StartNewFile(filename, false);

    public static void Process(this ITemplateFileManager instance)
        => instance.Process(true, false);

    public static void Process(this ITemplateFileManager instance, bool split)
        => instance.Process(split, false);

    public static void DeleteLastGeneratedFiles(this ITemplateFileManager instance, string lastGeneratedFilesPath)
        => instance.DeleteLastGeneratedFiles(lastGeneratedFilesPath, true);
}
