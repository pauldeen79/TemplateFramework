namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderTests
{
    public class Create : ProviderTests
    {
        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(request: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_On_Request_Other_Than_CreateCompiledTemplateRequest()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(request: new Mock<ICreateTemplateRequest>().Object))
               .Should().Throw<ArgumentException>().WithParameterName("request");
        }

        [Fact]
        public void Returns_Template_Instance_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var instance = sut.Create(new CreateCompiledTemplateRequest(GetType().Assembly.FullName!, GetType().FullName!));

            // Assert
            instance.Should().NotBeNull();
            instance.GetType().FullName.Should().Be(GetType().FullName);
        }

        [Fact]
        public void Throws_On_Wrong_ClassName()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(new CreateCompiledTemplateRequest(GetType().Assembly.FullName!, "WrongName")))
               .Should().Throw<NotSupportedException>();
        }
    }
}
