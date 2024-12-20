namespace TemplateFramework.Core.CodeGeneration;

internal class NamedResult<T>(string name, T result)
{
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));
    public T Result { get; } = result ?? throw new ArgumentNullException(nameof(result));
}
