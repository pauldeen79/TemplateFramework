namespace TemplateFramework.Abstractions.Templates;

public interface IParameterizedTemplate
{
    Task<Result> SetParameterAsync(string name, object? value, CancellationToken cancellationToken);
    Task<Result<ITemplateParameter[]>> GetParametersAsync(CancellationToken cancellationToken);
}
