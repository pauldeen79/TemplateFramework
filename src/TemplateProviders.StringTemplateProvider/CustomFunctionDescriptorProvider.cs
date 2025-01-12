namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class CustomFunctionDescriptorProvider : IFunctionDescriptorProvider
{
    private readonly IFunction[] _functions;

    public CustomFunctionDescriptorProvider(IEnumerable<IFunction> functions)
    {
        ArgumentGuard.IsNotNull(functions, nameof(functions));

        _functions = functions.ToArray();
    }

    public IReadOnlyCollection<FunctionDescriptor> GetAll()
        => _functions.SelectMany(x => CreateFunction(x, x.GetType())).ToList();

    internal static IEnumerable<FunctionDescriptor> CreateFunction(IFunction source, Type functionType)
    {
        var type = source.GetType();

        if (source is IDynamicDescriptorsFunction dynamicDescriptorsFunction)
        {
            foreach (var descriptor in dynamicDescriptorsFunction.GetDescriptors())
            {
                yield return descriptor;
            }
        }
        else
        {
            yield return new FunctionDescriptorBuilder()
                .WithName(type.GetCustomAttribute<FunctionNameAttribute>()?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal))
                .WithDescription(type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
                .WithFunctionType(functionType)
                .WithReturnValueType(type.GetCustomAttribute<FunctionResultTypeAttribute>()?.Type)
                .AddArguments(type.GetCustomAttributes<FunctionArgumentAttribute>().Select(CreateFunctionArgument))
                .AddResults(type.GetCustomAttributes<FunctionResultAttribute>().Select(CreateFunctionResult))
                .Build();
        }
    }

    private static FunctionDescriptorArgumentBuilder CreateFunctionArgument(FunctionArgumentAttribute attribute)
        => new FunctionDescriptorArgumentBuilder()
            .WithName(attribute.Name)
            .WithDescription(attribute.Description)
            .WithType(attribute.Type)
            .WithIsRequired(attribute.IsRequired);

    private static FunctionDescriptorResultBuilder CreateFunctionResult(FunctionResultAttribute attribute)
        => new FunctionDescriptorResultBuilder()
            .WithDescription(attribute.Description)
            .WithStatus(attribute.Status)
            .WithValue(attribute.Value)
            .WithValueType(attribute.ValueType);
}
