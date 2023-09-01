namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Abstractions;

public interface ICompiledTemplateFactory
{
    object Create(Type type);
}
