namespace TemplateFramework.TemplateProviders.ChildTemplateProvider;

public class TemplateCreator<T> : ITemplateCreator
    where T : class
{
    private readonly Func<T> _factory;
    private readonly Type? _modelType;
    private readonly string? _name;

    public TemplateCreator(Type modelType) : this(new Func<T>(Activator.CreateInstance<T>), modelType ?? throw new ArgumentNullException(nameof(modelType)), null)
    {
    }

    public TemplateCreator(string name) : this(new Func<T>(Activator.CreateInstance<T>), null, name ?? throw new ArgumentNullException(nameof(name)))
    {
    }

    public TemplateCreator(Func<T> factory, Type? modelType, string? name)
    {
        Guard.IsNotNull(factory);

        if (modelType is null && name is null)
        {
            throw new InvalidOperationException("Either modelType or name is required");
        }

        _modelType = modelType;
        _name = name;
        _factory = factory;
    }

    public Result<object> CreateByModel(object? model)
        => SupportsModel(model)
            ? Result.Success<object>(_factory()).EnsureNotNull().EnsureValue()
            : Result.Continue<object>();

    public Result<object> CreateByName(string name)
        => SupportsName(name)
            ? Result.Success<object>(_factory()).EnsureNotNull().EnsureValue()
            : Result.Continue<object>();

    public bool SupportsModel(object? model)
        => model is not null && _modelType?.IsInstanceOfType(model) == true;

    public bool SupportsName(string name)
        => _name == name;
}
