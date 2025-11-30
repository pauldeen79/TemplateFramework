namespace TemplateFramework.Core;

public sealed class TemplateInitializer : ITemplateInitializer
{
    private readonly IEnumerable<ITemplateInitializerComponent> _components;

    public TemplateInitializer(IEnumerable<ITemplateInitializerComponent> components)
    {
        Guard.IsNotNull(components);

        _components = components.OrderBy(x => x.Order);
    }

    public async Task<Result> InitializeAsync(ITemplateEngineContext context, CancellationToken token)
    {
        Guard.IsNotNull(context);

        foreach (var component in _components)
        {
            var result = await component.InitializeAsync(context, token).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }
}
