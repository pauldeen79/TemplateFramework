namespace TemplateFramework.Console.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Act
        var services = new ServiceCollection();
        services.InjectClipboard();
        using var provider = services
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateCommands()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        // Assert
        provider.Should().NotBeNull();
    }
}
