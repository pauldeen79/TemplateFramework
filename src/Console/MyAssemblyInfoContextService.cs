namespace TemplateFramework.Console;

public sealed class MyAssemblyInfoContextService : IAssemblyInfoContextService
{
    public string[] GetExcludedAssemblies() =>
    [
        "System.Runtime",
        "System.Collections",
        "System.ComponentModel",
        "TemplateFramework.Abstractions",
        "TemplateFramework.Console",
        "TemplateFramework.Core",
        "TemplateFramework.Core.CodeGeneration",
        "TemplateFramework.Runtime",
        "TemplateFramework.TemplateProviders.ChildTemplateProvider",
        "TemplateFramework.TemplateProviders.CompiledTemplateProvider",
        "TemplateFramework.TemplateProviders.StringTemplateProvider",
        "CrossCutting.Common",
        "CrossCutting.Utilities.Parsers",
        "Microsoft.Extensions.DependencyInjection",
        "Microsoft.Extensions.DependencyInjection.Abstractions"
    ];
}
