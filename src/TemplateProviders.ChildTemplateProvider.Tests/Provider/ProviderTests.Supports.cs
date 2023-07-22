namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderTests
{
    public class Supports : ProviderTests
    {
        [Fact]
        public void Returns_False_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();
            var request = (ICreateTemplateRequest)null!;

            // Act
            var result = sut.Supports(request);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_On_Unsupported_Request()
        {
            // Arrange
            var sut = CreateSut();
            var request = new Mock<ICreateTemplateRequest>().Object;

            // Act
            var result = sut.Supports(request);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_On_CreateTemplateByModelRequest()
        {
            // Arrange
            var sut = CreateSut();
            var request = new CreateTemplateByModelRequest(this);

            // Act
            var result = sut.Supports(request);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_True_On_CreateTemplateByNameRequest()
        {
            // Arrange
            var sut = CreateSut();
            var request = new CreateTemplateByNameRequest(nameof(Returns_True_On_CreateTemplateByNameRequest));

            // Act
            var result = sut.Supports(request);

            // Assert
            result.Should().BeTrue();
        }
    }
}
