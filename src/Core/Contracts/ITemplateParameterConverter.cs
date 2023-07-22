namespace TemplateFramework.Core.Contracts;

public interface ITemplateParameterConverter
{
    bool TryConvert(object? value, Type type, out object? convertedValue);
}
