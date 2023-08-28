namespace TemplateFramework.Console.Abstractions;

public interface IUserInput
{
    string GetValue(ITemplateParameter parameter);
}
