using CrossCutting.Common.Extensions;
using CrossCutting.Common;
using CrossCutting.Utilities.Parsers.Builders;
using System.ComponentModel;
using System.Reflection;

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
        => _functions.SelectMany(CreateFunction).ToList();

    private static IEnumerable<FunctionDescriptor> CreateFunction(IFunction source)
    {
        var type = source.GetType();

        //if (source is TemplateFrameworkContextFunction templateFrameworkContextFunction)
        //{
        //    foreach(var function in templateFrameworkContextFunction
        //}
        //else
        //{
            yield return new FunctionDescriptorBuilder()
                .WithName(type.GetCustomAttribute<FunctionNameAttribute>()?.Name ?? type.Name.ReplaceSuffix("Function", string.Empty, StringComparison.Ordinal))
                .WithDescription(type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty)
                .WithFunctionType(type)
                .WithReturnValueType(type.GetCustomAttribute<FunctionResultTypeAttribute>()?.Type)
                .AddArguments(type.GetCustomAttributes<FunctionArgumentAttribute>().Select(CreateFunctionArgument))
                .AddResults(type.GetCustomAttributes<FunctionResultAttribute>().Select(CreateFunctionResult))
                .Build();
        //}
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
