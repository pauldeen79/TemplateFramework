namespace TemplateFramework.Abstractions;

public interface ISessionAwareComponent
{
    Task<Result> StartSessionAsync(CancellationToken cancellationToken);
}
