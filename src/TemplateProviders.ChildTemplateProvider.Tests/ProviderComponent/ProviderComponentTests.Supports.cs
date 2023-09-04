namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class Supports : ProviderComponentTests
    {
        [Fact]
        public void Returns_False_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = (ITemplateIdentifier)null!;

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_On_Unsupported_Identifier()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = new Mock<ITemplateIdentifier>().Object;

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_On_CreateTemplateByModelRequest()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = new TemplateByModelIdentifier(this);
            TemplateCreatorMock.Setup(x => x.SupportsModel(It.IsAny<object?>())).Returns(true);

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_True_On_CreateTemplateByNameRequest()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = new TemplateByNameIdentifier(nameof(Returns_True_On_CreateTemplateByNameRequest));
            TemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>())).Returns(true);

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeTrue();
        }
    }
}
