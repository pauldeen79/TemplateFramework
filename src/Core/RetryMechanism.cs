namespace TemplateFramework.Core;

public class RetryMechanism : IRetryMechanism
{
    protected virtual int WaitTimeInMs => 500;

    public void Retry(Action action)
    {
        Guard.IsNotNull(action);

        for (int i = 1; i <= 3; i++)
        {
            try
            {
                action();
                return;
            }
            catch (IOException x) when (x.Message.Contains("because it is being used by another process", StringComparison.InvariantCulture))
            {
                Thread.Sleep(i * WaitTimeInMs);
            }
        }
    }
}
