namespace TemplateFramework.Abstractions;

public interface ISessionAwareComponent
{
    Task<Result> StartSession(CancellationToken cancellationToken);
}
