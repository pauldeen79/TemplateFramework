namespace TemplateFramework.Core;

public class TemplateContext : ITemplateContext
{
    public TemplateContext(object template)
        : this(template, null, null, null, null)
    {
    }

    public TemplateContext(object template, ITemplateContext parentContext)
        : this(template, null, parentContext, null, null)
    {
    }

    public TemplateContext(object template, object? model)
        : this(template, model, null, null, null)
    {
    }

    public TemplateContext(object template, object? model, ITemplateContext parentContext)
        : this(template, model, parentContext, null, null)
    {
    }

    public TemplateContext(object template,
                           object? model,
                           ITemplateContext? parentContext,
                           int? iterationNumber,
                           int? iterationCount)
    {
        Guard.IsNotNull(template);

        Template = template;
        Model = model;
        ParentContext = parentContext;
        IterationNumber = iterationNumber;
        IterationCount = iterationCount;
    }

    public object Template { get; }
    public object? Model { get; }
    public ITemplateContext? ParentContext { get; }

    public ITemplateContext RootContext
    {
        get
        {
            ITemplateContext? p = this;
            while (p.ParentContext is not null)
            {
                p = p.ParentContext;
            }

            return p;
        }
    }

    public T? GetModelFromContextByType<T>(Predicate<ITemplateContext>? predicate)
    {
        ITemplateContext? p = this;
        while (p is not null)
        {
            if (p.Model is T t && (predicate is null || predicate(p)))
            {
                return t;
            }

            p = p.ParentContext;
        }

        return default;
    }

    public T? GetContextByTemplateType<T>(Predicate<ITemplateContext>? predicate)
    {
        ITemplateContext? p = this;
        while (p is not null)
        {
            if (p.Template is T t && (predicate is null || predicate(p)))
            {
                return t;
            }

            p = p.ParentContext;
        }

        return default;
    }

    public bool IsRootContext => ParentContext is null;

    public ITemplateContext CreateChildContext(object template,
                                               object? model,
                                               int? iterationNumber,
                                               int? iterationCount)
    {
        Guard.IsNotNull(template);

        return new TemplateContext
        (
            template: template,
            model: model,
            parentContext: this,
            iterationNumber: iterationNumber,
            iterationCount: iterationCount
        );
    }

    public int? IterationNumber { get; set; }
    public int? IterationCount { get; set; }

    public bool HasIterations => IterationNumber is not null && IterationCount is not null;

    public bool? IsFirstIteration
    {
        get
        {
            if (IterationNumber is null || IterationCount is null)
            {
                return null;
            }

            return IterationNumber.Value == 0;
        }
    }

    public bool? IsLastIteration
    {
        get
        {
            if (IterationNumber is null || IterationCount is null)
            {
                return null;
            }

            return IterationNumber.Value + 1 == IterationCount.Value;
        }
    }

    public static ITemplateContext CreateRootContext(object template)
        => new TemplateContext
        (
            template: template
        );
}
