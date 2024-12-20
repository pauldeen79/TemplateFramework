namespace TemplateFramework.Core.CodeGeneration;

internal class NamedResult<T>(string name, T result)
{
    public string Name { get; } = name;
    public T Result { get; } = result;
}
