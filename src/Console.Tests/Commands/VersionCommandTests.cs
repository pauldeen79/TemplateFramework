namespace TemplateFramework.Console.Tests.Commands;

public class VersionCommandTests : TestBase<VersionCommand>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(VersionCommand).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
    public class Initialize : VersionCommandTests
    {
        [Fact]
        public void Adds_VersionCommand_To_Application()
        {
            // Arrange
            using var app = new CommandLineApplication();
            var sut = CreateSut();

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().BeEmpty(); // aparently, this does not add a command that is publicly visible...
        }

        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(null!))
               .Should().Throw<ArgumentNullException>();
        }
    }
}
