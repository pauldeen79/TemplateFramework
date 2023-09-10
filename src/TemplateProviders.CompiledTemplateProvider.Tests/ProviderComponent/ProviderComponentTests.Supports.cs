namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class Supports : ProviderComponentTests
    {
        [Fact]
        public void Returns_False_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_On_Request_Other_Than_CreateCompiledTemplateRequest()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(Substitute.For<ITemplateIdentifier>());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_On_CreateCompiledTemplateRequest()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!, string.Empty));

            // Assert
            result.Should().BeTrue();
        }
    }
}
