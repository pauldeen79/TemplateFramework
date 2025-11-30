namespace TemplateFramework.Core.Abstractions;

public interface IValueConverter
{
    Result<object?> Convert(object? value, Type type, ITemplateEngineContext context);
}
