namespace TemplateFramework.Abstractions;

public interface ITemplateContext
{
    object Template { get; }
    object? Model { get; }
    ITemplateContext? ParentContext { get; }
    ITemplateContext RootContext { get; }
    bool IsRootContext { get; }
    bool HasIterations { get; }
    int? IterationNumber { get; set; }
    int? IterationCount { get; set; }
    bool? IsFirstIteration { get; }
    bool? IsLastIteration { get; }

    T? GetModelFromContextByType<T>(Predicate<ITemplateContext>? predicate);
    T? GetContextByTemplateType<T>(Predicate<ITemplateContext>? predicate);

    ITemplateContext CreateChildContext(object template,
                                        object? model,
                                        int? iterationNumber,
                                        int? iterationCount);
}
