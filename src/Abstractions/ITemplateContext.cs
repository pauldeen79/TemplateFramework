﻿namespace TemplateFramework.Abstractions;

public interface ITemplateContext : IChildTemplateContext
{
    object Template { get; }
    ITemplateContext? ParentContext { get; }
    ITemplateContext RootContext { get; }
    ITemplateEngine Engine { get; }
    ITemplateComponentRegistry TemplateComponentRegistry { get; }
    string DefaultFilename { get; }
    bool IsRootContext { get; }
    bool HasIterations { get; }
    bool? IsFirstIteration { get; }
    bool? IsLastIteration { get; }

    T? GetModelFromContextByType<T>(Predicate<ITemplateContext>? predicate);
    T? GetContextByTemplateType<T>(Predicate<ITemplateContext>? predicate);

    ITemplateContext CreateChildContext(IChildTemplateContext childContext);
}
