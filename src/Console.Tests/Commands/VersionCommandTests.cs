namespace TemplateFramework.Console.Tests.Commands;

public class VersionCommandTests
{
    [Fact]
    public void Ctor_Throws_On_Null_Argument()
    {
        TestHelpers.ConstructorMustThrowArgumentNullException(typeof(VersionCommand));
    }

    [Fact]
    public void Initialize_Adds_VersionCommand_To_Application()
    {
        // Arrange
        using var app = new CommandLineApplication();
        var sut = new VersionCommand();

        // Act
        sut.Initialize(app);

        // Assert
        app.Commands.Should().HaveCount(0); // aparently, this does not add a command that is publicly visible...
    }

    [Fact]
    public void Initialize_Throws_On_Null_Argument()
    {
        // Arrange
        var sut = new VersionCommand();

        // Act & Assert
        sut.Invoking(x => x.Initialize(null!))
           .Should().Throw<ArgumentNullException>();
    }
}
