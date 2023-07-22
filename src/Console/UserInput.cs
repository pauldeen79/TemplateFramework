namespace TemplateFramework.Console;

[ExcludeFromCodeCoverage]
public class UserInput : IUserInput
{
    public string GetValue(TemplateParameter parameter)
    {
        Guard.IsNotNull(parameter);

        return parameter.Type.IsEnum
            ? Sharprompt.Prompt.Select(parameter.Name, ConvertToString(Enum.GetValues(parameter.Type)))
            : Sharprompt.Prompt.Input<string>(parameter.Name);
    }

    private static IEnumerable<string> ConvertToString(Array array)
    {
        foreach (var item in array)
        {
            yield return item.ToString()!;
        }
    }
}
