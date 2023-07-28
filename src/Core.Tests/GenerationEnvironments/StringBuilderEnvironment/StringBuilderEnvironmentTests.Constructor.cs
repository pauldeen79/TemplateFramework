namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class StringBuilderEnvironmentTests
{
    public class Constructor : StringBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Act & Assert
            this.Invoking(_ => new StringBuilderEnvironment(FileSystemMock.Object, builder: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Creates_Instance_Correctly()
        {
            // Act
            var instance = CreateSut();

            // Assert
            instance.Should().NotBeNull();
            instance.Builder.Should().BeSameAs(Builder);
        }
    }
}
