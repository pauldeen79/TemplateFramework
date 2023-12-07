namespace TemplateFramework.Core.Abstractions;

public interface IValueConverter
{
    object? Convert(object? value, Type type, ITemplateEngineContext context);
}
