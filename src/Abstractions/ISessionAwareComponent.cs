namespace TemplateFramework.Abstractions;

public interface ISessionAwareComponent
{
    Task StartSession(CancellationToken cancellationToken);
}
