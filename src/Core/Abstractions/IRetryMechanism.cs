namespace TemplateFramework.Core.Abstractions;

public interface IRetryMechanism
{
    void Retry(Action action);
}
