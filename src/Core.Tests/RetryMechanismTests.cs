namespace TemplateFramework.Core.Tests;

public class RetryMechanismTests
{
    [Fact]
    public void Retry_Retries_When_Exception_Occurs()
    {
        // Arrange
        var sut = new FastRetryMechanism();
        int counter = 0;

        // Act
        sut.Retry(() =>
        {
            counter++;

            if (counter == 1)
            {
                throw new IOException("Can't write to file because it is being used by another process");
            }
        });

        // Assert
        counter.Should().Be(2);
    }

    private sealed class FastRetryMechanism : RetryMechanism
    {
        protected override int WaitTimeInMs => 1;
    }
}
