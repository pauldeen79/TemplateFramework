namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ComponentRegistrationContextFunction : IDynamicDescriptorsFunction
{
    private readonly List<IFunction> _functions = new();
    private readonly IFunctionDescriptorMapper _functionDescriptorMapper;

    public ComponentRegistrationContextFunction(IFunctionDescriptorMapper functionDescriptorMapper)
    {
        Guard.IsNotNull(functionDescriptorMapper);

        _functionDescriptorMapper = functionDescriptorMapper;
    }

    public void AddFunction(IFunction function)
    {
        Guard.IsNotNull(function);

        _functions.Add(function);
    }

    public void ClearFunctions()
    {
        _functions.Clear();
    }

    public Result<object?> Evaluate(FunctionCallContext context)
    {
        Guard.IsNotNull(context);

        foreach (var function in _functions.Where(x => _functionDescriptorMapper.Map(x, x.GetType()).Any(y => y.Name.Equals(context.FunctionCall.Name, StringComparison.OrdinalIgnoreCase))))
        {
            var result = function.Evaluate(context);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.NotSupported<object?>($"Unknown function: {context.FunctionCall.Name}");
    }

    public IEnumerable<FunctionDescriptor> GetDescriptors()
    {
        return _functions.SelectMany(x => _functionDescriptorMapper.Map(x, GetType())).ToArray();
    }
}
