namespace TemplateFramework.Core.CodeGeneration;

[ExcludeFromCodeCoverage]
public sealed class CodeGenerationProviderWrapper : ICodeGenerationProvider
{
    private readonly object _wrappedInstance;

    public CodeGenerationProviderWrapper(object wrappedInstance) => _wrappedInstance = wrappedInstance ?? throw new ArgumentNullException(nameof(wrappedInstance));
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8605 // Unboxing a possibly null value.
    public bool GenerateMultipleFiles => (bool)_wrappedInstance.GetType().GetProperty(nameof(GenerateMultipleFiles)).GetValue(_wrappedInstance);

    public bool SkipWhenFileExists => (bool)_wrappedInstance.GetType().GetProperty(nameof(SkipWhenFileExists)).GetValue(_wrappedInstance);

    public string BasePath => (string)_wrappedInstance.GetType().GetProperty(nameof(BasePath)).GetValue(_wrappedInstance);

    public string Path => (string)_wrappedInstance.GetType().GetProperty(nameof(Path)).GetValue(_wrappedInstance);

    public string DefaultFilename => (string)_wrappedInstance.GetType().GetProperty(nameof(DefaultFilename)).GetValue(_wrappedInstance);

    public bool RecurseOnDeleteGeneratedFiles => (bool)_wrappedInstance.GetType().GetProperty(nameof(RecurseOnDeleteGeneratedFiles)).GetValue(_wrappedInstance);

    public string LastGeneratedFilesFilename => (string)_wrappedInstance.GetType().GetProperty(nameof(LastGeneratedFilesFilename)).GetValue(_wrappedInstance);

    public object? CreateAdditionalParameters() => _wrappedInstance.GetType().GetMethod(nameof(CreateAdditionalParameters)).Invoke(_wrappedInstance, Array.Empty<object>());

    public object CreateGenerator() => _wrappedInstance.GetType().GetMethod(nameof(CreateGenerator)).Invoke(_wrappedInstance, Array.Empty<object>());

    public object? CreateModel() => _wrappedInstance.GetType().GetMethod(nameof(CreateModel)).Invoke(_wrappedInstance, Array.Empty<object>());

    public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath) => _wrappedInstance.GetType().GetMethod(nameof(Initialize)).Invoke(_wrappedInstance, new object[] { generateMultipleFiles, skipWhenFileExists, basePath });
#pragma warning restore CS8605 // Unboxing a possibly null value.
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}
