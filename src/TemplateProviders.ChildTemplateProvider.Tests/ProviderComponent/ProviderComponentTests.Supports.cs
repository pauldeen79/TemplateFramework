namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class Supports : ProviderComponentTests
    {
        [Fact]
        public void Returns_False_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();
            var request = (ITemplateIdentifier)null!;

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
            var request = new Mock<ITemplateIdentifier>().Object;

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
            var request = new TemplateByModelIdentifier(this);

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
            var request = new TemplateByNameIdentifier(nameof(Returns_True_On_CreateTemplateByNameRequest));

            // Act
            var result = sut.Supports(request);

            // Assert
            result.Should().BeTrue();
        }
    }
}
