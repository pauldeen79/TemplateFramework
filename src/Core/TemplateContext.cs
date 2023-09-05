namespace TemplateFramework.Core;

public sealed class TemplateContext : ITemplateContext
{
    public TemplateContext(ITemplateEngine engine,
                           ITemplateComponentRegistry templateComponentRegistry,
                           string defaultFilename,
                           ITemplateIdentifier identifier,
                           object template)
        : this(engine, templateComponentRegistry, defaultFilename, identifier, template, null, null, null, null)
    {
    }

    public TemplateContext(ITemplateEngine engine,
                           ITemplateComponentRegistry templateComponentRegistry,
                           string defaultFilename,
                           ITemplateIdentifier identifier,
                           object template,
                           ITemplateContext parentContext)
        : this(engine, templateComponentRegistry, defaultFilename, identifier, template, null, parentContext, null, null)
    {
    }

    public TemplateContext(ITemplateEngine engine,
                           ITemplateComponentRegistry templateComponentRegistry,
                           string defaultFilename,
                           ITemplateIdentifier identifier,
                           object template,
                           object? model)
        : this(engine, templateComponentRegistry, defaultFilename, identifier, template, model, null, null, null)
    {
    }

    public TemplateContext(ITemplateEngine engine,
                           ITemplateComponentRegistry templateComponentRegistry,
                           string defaultFilename,
                           ITemplateIdentifier identifier,
                           object template,
                           object? model,
                           ITemplateContext parentContext)
        : this(engine, templateComponentRegistry, defaultFilename, identifier, template, model, parentContext, null, null)
    {
    }

#pragma warning disable S107
    public TemplateContext(ITemplateEngine engine,
                           ITemplateComponentRegistry templateComponentRegistry,
                           string defaultFilename,
                           ITemplateIdentifier identifier,
                           object template,
                           object? model,
                           ITemplateContext? parentContext,
                           int? iterationNumber,
                           int? iterationCount)
#pragma warning restore S107
    {
        Guard.IsNotNull(engine);
        Guard.IsNotNull(templateComponentRegistry);
        Guard.IsNotNull(defaultFilename);
        Guard.IsNotNull(identifier);
        Guard.IsNotNull(template);

        Engine = engine;
        TemplateComponentRegistry = templateComponentRegistry;
        DefaultFilename = defaultFilename;
        Identifier = identifier;
        Template = template;
        Model = model;
        ParentContext = parentContext;
        IterationNumber = iterationNumber;
        IterationCount = iterationCount;
    }

    public ITemplateIdentifier Identifier { get; }
    public object Template { get; }
    public object? Model { get; }
    public ITemplateContext? ParentContext { get; }
    public ITemplateEngine Engine { get; }
    public ITemplateComponentRegistry TemplateComponentRegistry { get; }
    public string DefaultFilename { get; }

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

    public ITemplateContext CreateChildContext(IChildTemplateContext childContext)
    {
        Guard.IsNotNull(childContext);

        return new TemplateContext
        (
            engine: Engine,
            templateComponentRegistry: TemplateComponentRegistry,
            defaultFilename: DefaultFilename,
            identifier: childContext.Identifier,
            template: new IgnoreThis(),
            model: childContext.Model,
            parentContext: this,
            iterationNumber: childContext.IterationNumber,
            iterationCount: childContext.IterationCount
        );
    }

    public int? IterationNumber { get; }
    public int? IterationCount { get; }

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
}
