namespace TemplateFramework.Abstractions.Templates;

public interface IParameterizedTemplate
{
    Task<Result> SetParameterAsync(string name, object? value, CancellationToken token);
    Task<Result<ITemplateParameter[]>> GetParametersAsync(CancellationToken token);
}
