﻿namespace TemplateFramework.Core.CodeGeneration.Tests;

internal static class TestData
{
#if Windows
    internal const string BasePath = @"C:\Somewhere";
#elif Linux
    internal const string BasePath = @"/usr/bin/python3";
#elif OSX
    internal const string BasePath = @"/Users/moi/Downloads";
#else
    internal const string BasePath = "Unknown basepath, only Windows, Linux and OSX are supported";
#endif
    
    internal static string GetAssemblyName() => typeof(TestData).Assembly.FullName!;
}

public sealed class MyGeneratorProvider : ICodeGenerationProvider
{
    public string Path { get; } = "";

    public bool RecurseOnDeleteGeneratedFiles { get; }

    public string LastGeneratedFilesFilename { get; } = "";

    public Encoding Encoding => Encoding.UTF8; 

    public Task<object?> CreateAdditionalParameters() => Task.FromResult(default(object?));

    public Type GetGeneratorType() => typeof(MyGenerator);

    public Task<object?> CreateModel() => Task.FromResult(default(object?));
}

public sealed class MyGenerator
{
    public override string ToString() => "Here is code from MyGenerator";
}
