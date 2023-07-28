namespace TemplateFramework.Core.Requests;

public class RenderTemplateRequest : IRenderTemplateRequest
{
    public RenderTemplateRequest(
        object template,
        object? model,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        StringBuilder builder)
        : this(template, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder)
        : this(template, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        StringBuilder builder,
        string defaultFilename)
        : this(template, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename)
        : this(template, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(template, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(template, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        StringBuilder builder,
        object? additionalParameters)
        : this(template, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        object? additionalParameters)
        : this(template, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        StringBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        StringBuilder builder,
        ITemplateContext? context)
        : this(template, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        ITemplateContext? context)
        : this(template, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IMultipleContentBuilder builder)
        : this(template, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder)
        : this(template, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename)
        : this(template, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename)
        : this(template, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(template, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(template, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IMultipleContentBuilder builder,
        object? additionalParameters)
        : this(template, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        object? additionalParameters)
        : this(template, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IMultipleContentBuilder builder,
        ITemplateContext? context)
        : this(template, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        ITemplateContext? context)
        : this(template, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        object? model,
        IGenerationEnvironment generationEnvironment,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(defaultFilename);

        Template = template;
        GenerationEnvironment = generationEnvironment;
        Model = model;
        DefaultFilename = defaultFilename;
        AdditionalParameters = additionalParameters;
        Context = context;
    }

    public object Template { get; }
    public IGenerationEnvironment GenerationEnvironment { get; }
    public object? Model { get; }
    public string DefaultFilename { get; }
    public object? AdditionalParameters { get; }
    public ITemplateContext? Context { get; }
}
