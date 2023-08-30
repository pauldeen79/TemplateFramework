namespace TemplateFramework.Core.CodeGeneration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFrameworkCodeGeneration(this IServiceCollection services)
        => services
            .AddScoped<ICodeGenerationAssembly, CodeGenerationAssembly>()
            .AddScoped<ICodeGenerationEngine, CodeGenerationEngine>()
            .AddScoped<ICodeGenerationProviderCreator, TypedCreator>()
            .AddScoped<ICodeGenerationProviderCreator, WrappedCreator>()
            ;
}
