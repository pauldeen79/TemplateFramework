namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider;

public class TemplateParameterWrapper : ITemplateParameter
{
    private readonly object _instance;

    public TemplateParameterWrapper(object instance)
    {
        Guard.IsNotNull(instance);

        _instance = instance;
    }

    public string Name
    {
        get
        {
            var prop = _instance.GetType().GetProperty(nameof(Name));
            if (prop is null)
            {
                throw new InvalidOperationException("TemplateParameter does not have a property called Name");
            }
            
            return prop.GetValue(_instance)?.ToString() ?? string.Empty;
        }
    }

    public Type Type
    {
        get
        {
            var prop = _instance.GetType().GetProperty(nameof(Type));
            if (prop is null)
            {
                throw new InvalidOperationException("TemplateParameter does not have a property called Type");
            }

            var result = prop.GetValue(_instance) as Type;
            if (result is null)
            {
                throw new InvalidOperationException("Type property did not return a Type instance");
            }

            return result;
        }
    }
}
