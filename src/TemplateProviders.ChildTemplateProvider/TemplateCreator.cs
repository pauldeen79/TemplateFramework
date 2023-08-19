namespace TemplateFramework.TemplateProviders.ChildTemplateProvider;

public class TemplateCreator<T> : ITemplateCreator
    where T : class, new()
{
    private readonly Type? _modelType;
    private readonly string? _name;

    public TemplateCreator(Type? modelType, string? name)
    {
        _modelType = modelType;
        _name = name;
    }

    public object CreateByModel(object? model)
        => SupportsModel(model)
            ? new T()
            : throw new NotSupportedException("Model type is not supported");

    public object CreateByName(string name)
        => SupportsName(name)
            ? new T()
            : throw new NotSupportedException("Name is not supported");

    public bool SupportsModel(object? model)
        => _modelType is not null
        && model is not null
        && _modelType.IsInstanceOfType(model);

    public bool SupportsName(string name)
        => _name == name;
}
