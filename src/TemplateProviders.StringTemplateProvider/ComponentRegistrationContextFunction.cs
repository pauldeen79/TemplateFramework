namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public class ComponentRegistrationContextFunction : IDynamicDescriptorsProvider, IFunction
{
    private readonly List<IFunction> _functions = new();
    private readonly IMemberDescriptorMapper _memberDescriptorMapper;

    public ComponentRegistrationContextFunction(IMemberDescriptorMapper memberDescriptorMapper)
    {
        Guard.IsNotNull(memberDescriptorMapper);

        _memberDescriptorMapper = memberDescriptorMapper;
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

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);

        var descriptorsResult = GetDescriptors();
        if (!descriptorsResult.IsSuccessful())
        {
            return descriptorsResult;
        }

        foreach (var function in _functions.Where(x => descriptorsResult.Value!.Any(y => y.Name.Equals(context.FunctionCall.Name, StringComparison.OrdinalIgnoreCase))))
        {
            var result = await function.EvaluateAsync(context, token).ConfigureAwait(false);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return Result.NotSupported<object?>($"Unknown function: {context.FunctionCall.Name}");
    }

    public Result<IReadOnlyCollection<CrossCutting.Utilities.ExpressionEvaluator.MemberDescriptor>> GetDescriptors()
    {
        var items = new List<CrossCutting.Utilities.ExpressionEvaluator.MemberDescriptor>();

        foreach (var function in _functions)
        {
            var result = _memberDescriptorMapper.Map(function, GetType());
            if (!result.EnsureValue().IsSuccessful())
            {
                return result;
            }

            items.AddRange(result.Value!);
        }

        return Result.Success<IReadOnlyCollection<CrossCutting.Utilities.ExpressionEvaluator.MemberDescriptor>>(items);
    }
}
