namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class StringBuilderEnvironmentTests
{
    public class Constructor : StringBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(StringBuilderEnvironment).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Fact]
        public void Creates_Instance_Correctly_With_Builder_Argument()
        {
            // Act
            var instance = CreateSut();

            // Assert
            instance.Should().NotBeNull();
            instance.Builder.Should().BeSameAs(Builder);
        }

        [Fact]
        public void Creates_Instance_Correctly_Without_Arguments()
        {
            // Act
            var instance = new StringBuilderEnvironment();

            // Assert
            instance.Builder.Should().NotBeNull();
        }
    }
}
