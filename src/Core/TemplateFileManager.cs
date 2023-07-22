namespace TemplateFramework.Core;

public class TemplateFileManager : ITemplateFileManager
{
    private readonly StringBuilder _originalStringBuilder;

    public TemplateFileManager()
        : this(new MultipleContentBuilder(), new StringBuilder())
    {
    }

    public TemplateFileManager(StringBuilder stringBuilder)
        : this(new MultipleContentBuilder(), stringBuilder)
    {
    }

    public TemplateFileManager(IMultipleContentBuilder multipleContentBuilder)
        : this(multipleContentBuilder, new StringBuilder())
    {
    }

    public TemplateFileManager(IMultipleContentBuilder multipleContentBuilder, StringBuilder stringBuilder)
    {
        Guard.IsNotNull(multipleContentBuilder);
        Guard.IsNotNull(stringBuilder);

        MultipleContentBuilder = multipleContentBuilder;
        _originalStringBuilder = stringBuilder;
    }

    public IMultipleContentBuilder MultipleContentBuilder { get; }
    public StringBuilder GenerationEnvironment { get; private set; } = new StringBuilder();

    public StringBuilder StartNewFile(string filename, bool skipWhenFileExists)
    {
        Guard.IsNotNull(filename);

        var currentContent = MultipleContentBuilder.AddContent(filename, skipWhenFileExists, new StringBuilder());
        GenerationEnvironment = currentContent.Builder;
        return currentContent.Builder;
    }

    public void ResetToDefaultOutput() => GenerationEnvironment = _originalStringBuilder;

    public void Process(bool split, bool silentOutput)
    {
        ResetToDefaultOutput();

        if (split)
        {
            _originalStringBuilder.Clear();
            if (!silentOutput)
            {
                var output = MultipleContentBuilder.ToString();
                if (!string.IsNullOrEmpty(output))
                {
                    _originalStringBuilder.Append(output);
                }
            }
        }
        else if (!silentOutput)
        {
            foreach (var item in MultipleContentBuilder.Contents)
            {
                _originalStringBuilder.Append(item.Builder);
            }
        }
    }

    public void SaveAll() => MultipleContentBuilder.SaveAll();

    public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
    {
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);
        MultipleContentBuilder.SaveLastGeneratedFiles(lastGeneratedFilesPath);
    }

    public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse)
    {
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);
        MultipleContentBuilder.DeleteLastGeneratedFiles(lastGeneratedFilesPath, recurse);
    }
}
