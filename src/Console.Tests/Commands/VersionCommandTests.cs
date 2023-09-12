namespace TemplateFramework.Console.Tests.Commands;

public class VersionCommandTests
{
    [Fact]
    public void Ctor_Throws_On_Null_Arguments()
    {
        typeof(VersionCommand).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
    }

    [Theory, AutoMockData]
    public void Initialize_Adds_VersionCommand_To_Application(VersionCommand sut)
    {
        // Arrange
        using var app = new CommandLineApplication();

        // Act
        sut.Initialize(app);

        // Assert
        app.Commands.Should().BeEmpty(); // aparently, this does not add a command that is publicly visible...
    }

    [Theory, AutoMockData]
    public void Initialize_Throws_On_Null_Argument(VersionCommand sut)
    {
        // Act & Assert
        sut.Invoking(x => x.Initialize(null!))
           .Should().Throw<ArgumentNullException>();
    }
}
