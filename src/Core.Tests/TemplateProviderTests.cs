namespace TemplateFramework.Core.Tests;

public class TemplateProviderTests
{
    protected TemplateProvider CreateSut() => new(new[] { TemplateProviderComponentMock.Object });

    protected Mock<ITemplateProviderComponent> TemplateProviderComponentMock { get; } = new();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Components()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateProvider(components: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("components");
        }
    }
    public class Create : TemplateProviderTests
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
        public void Throws_On_Unsupported_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(request: new Mock<ICreateTemplateRequest>().Object))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Returns_Template_Instance_From_Provider_On_Supported_Type()
        {
            // Arrange
            var sut = CreateSut();
            var request = new Mock<ICreateTemplateRequest>().Object;
            var expectedTemplate = new object();
            TemplateProviderComponentMock.Setup(x => x.Supports(request)).Returns(true);
            TemplateProviderComponentMock.Setup(x => x.Create(request)).Returns(expectedTemplate);

            // Act
            var template = sut.Create(request);

            // Assert
            template.Should().BeSameAs(expectedTemplate);
        }
    }
}
