﻿namespace TemplateFramework.Core.Requests;

public class RenderTemplateRequest : IRenderTemplateRequest
{
    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(identifier, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(identifier, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        StringBuilder builder)
        : this(identifier, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        StringBuilder builder)
        : this(identifier, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        StringBuilder builder,
        string defaultFilename)
        : this(identifier, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        StringBuilder builder,
        string defaultFilename)
        : this(identifier, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(identifier, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(identifier, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        StringBuilder builder,
        object? additionalParameters)
        : this(identifier, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        StringBuilder builder,
        object? additionalParameters)
        : this(identifier, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        StringBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(identifier, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        StringBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(identifier, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        StringBuilder builder,
        ITemplateContext? context)
        : this(identifier, model, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        StringBuilder builder,
        ITemplateContext? context)
        : this(identifier, null, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(identifier, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(identifier, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IMultipleContentBuilder builder)
        : this(identifier, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        IMultipleContentBuilder builder)
        : this(identifier, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename)
        : this(identifier, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        IMultipleContentBuilder builder,
        string defaultFilename)
        : this(identifier, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(identifier, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters)
        : this(identifier, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IMultipleContentBuilder builder,
        object? additionalParameters)
        : this(identifier, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        IMultipleContentBuilder builder,
        object? additionalParameters)
        : this(identifier, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, additionalParameters, null)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IMultipleContentBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(identifier, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        IMultipleContentBuilder builder,
        string defaultFilename,
        ITemplateContext? context)
        : this(identifier, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IMultipleContentBuilder builder,
        ITemplateContext? context)
        : this(identifier, model, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        IMultipleContentBuilder builder,
        ITemplateContext? context)
        : this(identifier, null, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), string.Empty, null, context)
    {
    }

    public RenderTemplateRequest(
        ITemplateIdentifier identifier,
        object? model,
        IGenerationEnvironment generationEnvironment,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
    {
        Guard.IsNotNull(identifier);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(defaultFilename);

        Identifier = identifier;
        GenerationEnvironment = generationEnvironment;
        Model = model;
        DefaultFilename = defaultFilename;
        AdditionalParameters = additionalParameters;
        Context = context;
    }

    public ITemplateIdentifier Identifier { get; }
    public IGenerationEnvironment GenerationEnvironment { get; }
    public object? Model { get; }
    public string DefaultFilename { get; }
    public object? AdditionalParameters { get; }
    public ITemplateContext? Context { get; }
}
