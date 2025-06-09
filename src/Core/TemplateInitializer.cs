namespace TemplateFramework.Core;

public sealed class TemplateInitializer : ITemplateInitializer
{
    private readonly IEnumerable<ITemplateInitializerComponent> _components;

    public TemplateInitializer(IEnumerable<ITemplateInitializerComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components.OrderBy(x => x.Order);
    }

    public async Task<Result> Initialize(ITemplateEngineContext context, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(context);

        var results = await Task.WhenAll(_components.Select(component => component.Initialize(context, cancellationToken))).ConfigureAwait(false);
        return Result.Aggregate(results, Result.Success(), nonSuccesfulResults => Result.Error(nonSuccesfulResults, "One or more template initializer components returned a non-succesful result, see the inner results for more details"));
    }
}
