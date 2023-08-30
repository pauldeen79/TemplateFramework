namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class Create : ProviderComponentTests
    {
        public Create()
        {
            AssemblyServiceMock
                .Setup(x => x.GetAssembly(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetType().Assembly);
        }

        [Fact]
        public void Throws_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Throws_On_Identifier_Other_Than_CreateCompiledTemplateRequest()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: new Mock<ITemplateIdentifier>().Object))
               .Should().Throw<ArgumentException>().WithParameterName("identifier");
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
            instance.Should().BeOfType<TemplateWrapper>();
        }

        [Fact]
        public void Throws_On_Wrong_ClassName()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(new CreateCompiledTemplateRequest(GetType().Assembly.FullName!, "WrongName")))
               .Should().Throw<InvalidOperationException>();
        }
    }
}
