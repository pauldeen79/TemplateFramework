namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    public class Constructor : MultipleContentBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentBuilderEnvironment(FileSystemMock.Object, RetryMechanism, builder: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Creates_Instance_Correctly_With_Builder_Argument()
        {
            // Act
            var instance = CreateSut();

            // Assert
            instance.Builder.Should().BeSameAs(MultipleContentBuilderMock.Object);
        }

        [Fact]
        public void Creates_Instance_Correctly_Without_Arguments()
        {
            // Act
            var instance = new MultipleContentBuilderEnvironment();

            // Assert
            instance.Builder.Should().NotBeNull();
        }
    }
}
