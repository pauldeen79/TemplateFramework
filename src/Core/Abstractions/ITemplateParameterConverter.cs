namespace TemplateFramework.Core.Abstractions;

public interface ITemplateParameterConverter
{
    Result<object?> Convert(object? value, Type type, ITemplateEngineContext context);
}
