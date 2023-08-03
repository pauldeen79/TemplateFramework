namespace TemplateFramework.Core.CodeGeneration;

internal sealed class CodeGenerationProviderWrapper : ICodeGenerationProvider
{
    private readonly object _instance;

    public CodeGenerationProviderWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        _instance = instance;
    }

    public string Path => NullGuard((string?)_instance.GetType().GetProperty(nameof(Path))?.GetValue(_instance), nameof(Path));

    public bool RecurseOnDeleteGeneratedFiles => NullGuard((bool?)_instance.GetType().GetProperty(nameof(RecurseOnDeleteGeneratedFiles))?.GetValue(_instance), nameof(RecurseOnDeleteGeneratedFiles));

    public string LastGeneratedFilesFilename => NullGuard((string?)_instance.GetType().GetProperty(nameof(LastGeneratedFilesFilename))?.GetValue(_instance), nameof(LastGeneratedFilesFilename));

    public Encoding Encoding => NullGuard((Encoding?)_instance.GetType().GetProperty(nameof(Encoding))?.GetValue(_instance), nameof(Encoding));

    public object? CreateAdditionalParameters() => _instance.GetType().GetMethod(nameof(CreateAdditionalParameters))?.Invoke(_instance, Array.Empty<object>());

    public object CreateGenerator() => NullGuard(_instance.GetType().GetMethod(nameof(CreateGenerator))?.Invoke(_instance, Array.Empty<object>()), nameof(CreateGenerator));

    public object? CreateModel() => _instance.GetType().GetMethod(nameof(CreateModel))?.Invoke(_instance, Array.Empty<object>());

    private T NullGuard<T>(T? value, string name)
    {
        if (value is null)
        {
            throw new InvalidOperationException($"{name} of template {_instance.GetType().FullName} was null");
        }

        return value;
    }

    private T NullGuard<T>(T? value, string name) where T : struct
    {
        if (value is null)
        {
            throw new InvalidOperationException($"{name} of template {_instance.GetType().FullName} was null");
        }

        return value.Value;
    }
}
