namespace TemplateFramework.Core.Requests;

public class RenderTemplateRequest : IRenderTemplateRequest
{
    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        object? additionalParameters)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        string defaultFilename)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        string defaultFilename,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
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
        DefaultFilename = defaultFilename;
        AdditionalParameters = additionalParameters;
        Context = context;
    }

    public object Template { get; }

    public IGenerationEnvironment GenerationEnvironment { get; }

    public string DefaultFilename { get; }

    public object? AdditionalParameters { get; }

    public ITemplateContext? Context { get; }
}

public class RenderTemplateRequest<TModel> : RenderTemplateRequest, IRenderTemplateRequest<TModel>
{
    public RenderTemplateRequest(IRenderTemplateRequest request)
        : this((request ?? throw new ArgumentNullException(nameof(request))).Template, request.GenerationEnvironment, default, request.DefaultFilename, request.AdditionalParameters, request.Context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        TModel? model,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        TModel? model)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        TModel? model,
        string defaultFilename)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        TModel? model,
        string defaultFilename,
        object? additionalParameters)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        TModel? model,
        object? additionalParameters)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        TModel? model,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        TModel? model,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        TModel? model,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        TModel? model)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        TModel? model,
        string defaultFilename)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        TModel? model,
        string defaultFilename,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        TModel? model,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        TModel? model,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        TModel? model,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), model, string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        TModel? model,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), model, defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        TModel? model)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), model, string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        TModel? model,
        string defaultFilename)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), model, defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        TModel? model,
        string defaultFilename,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), model, defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        TModel? model,
        object? additionalParameters)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), model, string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        TModel? model,
        string defaultFilename,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), model, defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        TModel? model,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), model, string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IGenerationEnvironment generationEnvironment,
        TModel? model,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context) : base(template, generationEnvironment, defaultFilename, additionalParameters, context)
    {
        Model = model;
    }

    public TModel? Model { get; }
}
