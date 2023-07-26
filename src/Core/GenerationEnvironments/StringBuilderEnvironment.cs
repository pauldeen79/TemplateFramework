﻿namespace TemplateFramework.Core.GenerationEnvironments;

public sealed class StringBuilderEnvironment : GenerationEnvironmentBase
{
    public StringBuilderEnvironment(StringBuilder builder) : this(new FileSystem(), builder)
    {
    }

    internal StringBuilderEnvironment(IFileSystem fileSystem, StringBuilder builder)
        : base(GenerationEnvironmentType.StringBuilder)
    {
        Guard.IsNotNull(builder);

        _fileSystem = fileSystem;
        Builder = builder;
    }

    private readonly IFileSystem _fileSystem;

    public StringBuilder Builder { get; }

    public override void Process(ICodeGenerationProvider provider, string basePath)
    {
        Guard.IsNotNull(provider);

        _fileSystem.WriteAllText(string.IsNullOrEmpty(basePath) ? provider.DefaultFilename : Path.Combine(basePath, provider.DefaultFilename), Builder.ToString(), provider.Encoding);
    }
}
